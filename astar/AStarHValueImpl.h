/*
 * AStarHValueImpl.h
 *
 *  Created on: 11 May 2016
 *      Author: w
 */

#ifndef ASTARHVALUEIMPL_H_
#define ASTARHVALUEIMPL_H_

#include "AStarHValue.h"

/*
 * Default implement of AStarHValue
 *
 * 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0
 * 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0
 * 0 0 0 1 - - - - - - - - - - - 0 0 0
 * 0 0 0 0 0 0 0 0 0 0 0 0 0 0 - 0 0 0
 * 0 0 0 0 0 0 0 0 0 0 0 0 0 0 - 0 0 0
 * 0 0 0 0 0 0 0 0 0 0 0 0 0 0 - 0 0 0
 * 0 0 0 0 0 0 0 0 0 0 0 0 0 0 - 0 0 0
 * 0 0 0 0 0 0 0 0 0 0 0 0 0 0 - 0 0 0
 * 0 0 0 0 0 0 0 0 0 0 0 0 0 0 2 0 0 0
 * 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0
 * 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0
 *
 */
class AStarHValueImpl : public AStarHValue
{

public:
    virtual int getHValue(AStarNode *pNode1, AStarNode *pNode2);
    static AStarHValueImpl* getInstance();

private:
    // Singleton
    AStarHValueImpl();
    virtual ~AStarHValueImpl();
    static AStarHValueImpl* instance;
};

#endif /* ASTARHVALUEIMPL_H_ */