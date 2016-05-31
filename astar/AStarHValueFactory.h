/*
 * AStarHValue.h
 *
 *  Created on: 11 May 2016
 *      Author: wtm11112@gmail.com
 */

#ifndef ASTARHVALUEFACTORY_H_
#define ASTARHVALUEFACTORY_H_

#include "AStarHValue.h"
#include "AStarHValueImpl.h"

enum HValueCalculatorType
{
    DEFAULT
};

class AStarHValueFactory
{
public:
    static AStarHValue* getHValueCalculator(HValueCalculatorType type)
    {
        switch (type)
        {
        case DEFAULT:
            return AStarHValueImpl::getInstance();
        }
        return nullptr;
    }
};

#endif /* ASTARHVALUEFACTORY_H_ */