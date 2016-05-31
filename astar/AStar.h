/*
 * File:   AStar.h
 * Author: wtm11112@gmail.com
 *
 * Created on 10 May, 2016, 8:51 PM
 */

#ifndef ASTAR_H
#define ASTAR_H

#include <list>
#include <vector>

#include "AStarNode.h"

class AStar {
public:
    AStar();

    virtual ~AStar();

    /*
     * init map based on the given array
     * return success
     */
    bool init(short *map, short *directionsMap, int row, int column);

    /*
     * look for the path
     * return the route list, nullptr if failed.
     */
    std::list<AStarNode*> *search();

    void print();

    void release();

private:
    static const int DISTANCE_BETWEEN_NODES = 1;
    
    std::vector<AStarNode*> getNeighbors(AStarNode* node);
    AStarNode* getMinFNodeFromOpen();

    AStarNode *startNode;
    AStarNode *targetNode;

    std::vector<std::vector<AStarNode *>> nodes;

    std::list<AStarNode*> openList;
    std::list<AStarNode*> closeList;
    
    std::list<AStarNode*>* pathList;
    
    int width;
    int height;
};

#endif /* ASTAR_H */