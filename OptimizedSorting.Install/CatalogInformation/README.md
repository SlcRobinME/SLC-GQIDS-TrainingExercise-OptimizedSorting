# Training Exercise GQI Ad Hoc Data Source Optimized Sorting

The goal of this exercise is to adapt this GQI (Generic Query Interface) ad hoc data source, to be able to sort on database-layer.

## Description

The ad hoc data source must be able to translate a sort operator coming from GQI to a DOM query.

1. Implement the `IGQIOptimizableDataSource` interface.
1. Store the sort operator, from the `Optimize` method.
1. Add a mapping between the `GQIColumns` and the correct Exposer.
1. Implement the `IGQIOnPrepareFetch` life cycle hook, to optimize the sort.

## Getting Started

For this exercise you'll start from an already existing Data-Source. You can install it by deploying this package.

This will deploy the following:

1. A dom module called `(exercise)tasks`,
1. 50 Dummy tasks and
1. the initial Data-Source

From here you can start on the actual implementation. The source code is available by clicking the Source button on the right. The data source is available in the `OptimizedSorting.GQI` project.