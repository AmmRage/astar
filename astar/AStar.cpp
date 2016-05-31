/*
 * File:   AStar.cpp
 * Author: wtm11112@gmail.com
 *
 * Created on 10 May, 2016, 8:51 PM
 */

#include <algorithm>
#include <climits>
#include <iostream>

#include "AStar.h"
#include "AStarHValue.h"
#include "AStarHValueFactory.h"

AStar::AStar()
    : startNode(nullptr)
    , targetNode(nullptr)
    , pathList(nullptr)
    , width(0)
    , height(0)
{
    this->pathList = new std::list<AStarNode*>();
}

AStar::~AStar() {
    if (this->pathList != nullptr) {
        this->pathList->clear();
        delete this->pathList;
        this->pathList = nullptr;
    }
    this->startNode = nullptr;
    this->targetNode = nullptr;
    this->height = 0;
    this->width = 0;
}

bool AStar::init(short *map, short *directionsMap, int row, int column) {

    auto hasStart = false;
    auto hasTarget = false;
    if (row <= 0 || column <= 0) {
        return false;
    }
    for (int i = 0; i < row; i++) {
        for (int j = 0; j < column; j++) {
            if (map[column * i + j] == 2) {
                if (hasStart) {
                    // only one start is allowed
                    return false;
                }
                hasStart = true;
            } else if (map[column * i + j] == 3) {
                if (hasTarget) {
                    // only one end is allowed
                    return false;
                }
                hasTarget = true;
            }
        }
    }
    if (!hasStart || !hasTarget) {
        // start or target is not found, input data verify failed.
        return false;
    }
    this->height = row;
    this->width = column;

    for (int i = 0; i < row; i++) {
        std::vector<AStarNode*> vec;

        for (int j = 0; j < column; j++) {
            auto node = new AStarNode(map[column * i + j], directionsMap[column * i + j], j, i);

            if (node->GetType() == START) {
                this->startNode = node;
            } else if (node->GetType() == TARGET) {
                this->targetNode = node;
            }

            vec.push_back(node);
        }
        nodes.push_back(vec);
    }
    this->startNode->SetH(AStarHValueFactory::getHValueCalculator(DEFAULT)->getHValue(this->startNode, this->targetNode));
    this->startNode->SetG(0);
    this->startNode->SetF(this->startNode->GetH() + this->startNode->GetG());
    this->openList.push_back(this->startNode);
    return true;
}

std::list<AStarNode*> * AStar::search() {
    
    while (!this->openList.empty()) {
        auto currentNode = getMinFNodeFromOpen();
        openList.remove(currentNode);
        this->pathList->clear();

        if (currentNode == this->targetNode) {
            // found route, build & return it.
            this->pathList->push_front(currentNode);
            currentNode = currentNode->GetPreNode();
            while (currentNode != this->startNode) {
                
                this->pathList->push_front(currentNode);
                auto node = this->nodes[currentNode->GetY()][currentNode->GetX()];
                node->SetType(ROUTE);
                currentNode = currentNode->GetPreNode();
            }
            this->pathList->push_front(currentNode);
            return this->pathList;
        }
        auto neighbors = this->getNeighbors(currentNode);

        for (auto &node : neighbors) {

            auto inOpenList = (std::find(openList.begin(), openList.end(), node) != openList.end());
            auto inCloseList = (std::find(closeList.begin(), closeList.end(), node) != closeList.end());

            auto g = currentNode->GetG() + DISTANCE_BETWEEN_NODES;
            auto h = AStarHValueFactory::getHValueCalculator(DEFAULT)->getHValue(this->targetNode, node);
            auto f = g + h;

            if ((!inOpenList && !inCloseList) || f < node->GetF()) {
                node->SetG(g);
                node->SetH(h);
                node->SetF(f);
                node->SetPreNode(currentNode);
                if (!inOpenList) {
                    openList.push_front(node);
                }
            }
            if (inCloseList && f < node->GetF()) {
                closeList.remove(node);
            }
        }
        // TODO use push_front() or push_back() for better performance.
        closeList.push_front(currentNode);
    }
    return this->pathList;
}

AStarNode* AStar::getMinFNodeFromOpen() {
    AStarNode *ret = nullptr;
    int minf = INT_MAX;

    for (auto &temp : openList) {
        if (temp->GetF() < minf) {
            minf = temp->GetF();
            ret = temp;
        }
    }
    return ret;
}

std::vector<AStarNode*> AStar::getNeighbors(AStarNode* node) {

	std::vector<AStarNode*> ret;
    if (node == nullptr) {
        return ret;
    }

    auto nodeX = node->GetX();
    auto nodeY = node->GetY();

    auto directions = node->GetDirections();
    for (auto const& direction: directions) {
        if (direction == UP) {
            // up
            if (nodeY > 0) {
                auto up = this->nodes[nodeY - 1][nodeX];
                if (up->GetType() != BLOCK) {
                    ret.push_back(up);
                }
            }
        } else if (direction == DOWN) {
            // down
            if (nodeY < static_cast<int>(this->nodes.size()) - 1) {
                auto down = this->nodes[nodeY + 1][nodeX];
                if (down->GetType() != BLOCK) {
                    ret.push_back(down);
                }
            }
        } else if (direction == LEFT) {
            // left
            if (nodeX > 0) {
                auto left = this->nodes[nodeY][nodeX - 1];
                if (left->GetType() != BLOCK) {
                    ret.push_back(left);
                }
            }
        } else if (direction == RIGHT) {
            // right
            if (nodeX < static_cast<int> (this->nodes[nodeY].size()) - 1) {
                auto right = this->nodes[nodeY][nodeX + 1];
                if (right->GetType() != BLOCK) {
                    ret.push_back(right);
                }
            }
        }
    }

    return ret;
}

void AStar::print() {
    for (auto const& tempNodes: this->nodes) {
        for (auto const& node: tempNodes) {
            char out = 0;
            switch (node->GetType()) {
                case AVAILABLE:
                    out = 'O';
                    break;
                case BLOCK:
                    out = '#';
                    break;
                case START:
                    out = 'S';
                    break;
                case TARGET:
                    out = 'E';
                    break;
                case ROUTE:
                    out = '`';
                    break;
            }
            std::cout << out << ' ';
        }
        std::cout << std::endl;
    }
}

void AStar::release() {
    for (auto &tempNodes: this->nodes) {
        for (auto &node: tempNodes) {
            delete node;
            node = nullptr;
        }
        tempNodes.clear();
    }
    nodes.clear();
    this->openList.clear();
    this->closeList.clear();
    this->pathList->clear();
}