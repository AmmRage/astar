/*
 * AStarHValue.h
 *
 *  Created on: 11 May 2016
 *      Author: wtm11112@gmail.com
 */

#ifndef ASTARHVALUE_H_
#define ASTARHVALUE_H_

#include "AStarNode.h"

class AStarHValue {
public:
    virtual ~AStarHValue(){};
    virtual int getHValue(AStarNode *pNode1, AStarNode *pNode2) = 0;
};

#endif /* ASTARHVALUE_H_ */