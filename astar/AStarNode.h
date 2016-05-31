/*
 * File:   AStarNode.h
 * Author: wtm11112@gmail.com
 *
 * Created on 10 May, 2016, 8:50 PM
 */

#ifndef ASTARNODE_H
#define ASTARNODE_H

#include <list>

enum AStarDirection {
    UP = 0,
    DOWN,
    LEFT,
    RIGHT
};

enum AStarNodeType {
    AVAILABLE = 0,
    BLOCK,
    START,
    TARGET,
    ROUTE
};

class AStarNode {
public:

    static const short DIRECTION_UP    = 0x01;
    static const short DIRECTION_DOWN  = 0x02;
    static const short DIRECTION_LEFT  = 0x04;
    static const short DIRECTION_RIGHT = 0x08;

    AStarNode();
    AStarNode(short type, short direction, int x, int y);

    virtual ~AStarNode();

    inline std::list<AStarDirection> GetDirections() const {
        return directions;
    }

    inline int GetF() const {
        return f;
    }

    inline void SetF(int f) {
        this->f = f;
    }

    inline int GetG() const {
        return g;
    }

    inline void SetG(int g) {
        this->g = g;
    }

    inline int GetH() const {
        return h;
    }

    inline void SetH(int h) {
        this->h = h;
    }

    inline AStarNode* GetPreNode() const {
        return preNode;
    }

    inline void SetPreNode(AStarNode* preNode) {
        this->preNode = preNode;
    }

    inline AStarNodeType GetType() const {
        return type;
    }

    inline void SetType(AStarNodeType type) {
        this->type = type;
    }

    inline int GetX() const {
        return x;
    }

    inline void SetX(int x) {
        this->x = x;
    }

    inline int GetY() const {
        return y;
    }

    inline void SetY(int y) {
        this->y = y;
    }

private:
    std::list<AStarDirection> directions;

    AStarNode* preNode;

    AStarNodeType type;

    int f;
    int g;
    int h;

    int x; // x-pos of the map
    int y; // y-pos of the map
};

#endif /* ASTARNODE_H */