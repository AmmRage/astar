/*
 * File:   AStarNode.cpp
 * Author: wtm11112@gmail.com
 *
 * Created on 10 May, 2016, 8:50 PM
 */

#include "AStarNode.h"

AStarNode::AStarNode()
    : preNode(nullptr)
    , type(BLOCK)
    , f(0)
    , g(0)
    , h(0)
    , x(0)
    , y(0)
{
}

AStarNode::AStarNode(short type, short direction, int x, int y)
    : preNode(nullptr)
    , f(0)
    , g(0)
    , h(0)
{
    if (type == 0) {
        this->type = AVAILABLE;
    } else if (type == 1) {
        this->type = BLOCK;
    } else if (type == 2) {
        this->type = START;
    } else if (type == 3) {
        this->type = TARGET;
    }

    if ((direction & DIRECTION_UP) != 0x00) {
        this->directions.push_back(UP);
    }
    if ((direction & DIRECTION_DOWN) != 0x00) {
        this->directions.push_back(DOWN);
    }
    if ((direction & DIRECTION_LEFT) != 0x00) {
        this->directions.push_back(LEFT);
    }
    if ((direction & DIRECTION_RIGHT) != 0x00) {
        this->directions.push_back(RIGHT);
    }

    this->x = x;
    this->y = y;
}

AStarNode::~AStarNode() {
    this->preNode = nullptr;
    this->f = 0;
    this->g = 0;
    this->h = 0;
    this->x = 0;
    this->y = 0;
    this->directions.clear();
}