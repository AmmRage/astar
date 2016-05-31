/*
 * AStarHValueImpl.cpp
 *
 *  Created on: 11 May 2016
 *      Author: w
 */

#include <cstdlib>

#include "AStarHValueImpl.h"
#include "AStarNode.h"

AStarHValueImpl* AStarHValueImpl::instance = nullptr;

AStarHValueImpl::AStarHValueImpl() {
}

AStarHValueImpl::~AStarHValueImpl() {
}

int AStarHValueImpl::getHValue(AStarNode *pNode1, AStarNode *pNode2)
{
    if (pNode1 == nullptr || pNode2 == nullptr) {
        return -1;
    }
    auto absX = abs(pNode1->GetX() - pNode2->GetX());
    auto absY = abs(pNode1->GetY() - pNode2->GetY());
    return absX + absY;
}

AStarHValueImpl* AStarHValueImpl::getInstance()
{
    if (instance == nullptr) {
        instance = new AStarHValueImpl();
    }
    return instance;
}