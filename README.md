# Training Exercise GQI Ad Hoc Data Source Optimized Sorting

## Description

The goal of this exercise is to adapt this GQI (Generic Query Interface) ad hoc data source, to be able to sort on database-layer.

The ad hoc data source must be able to translate a sort operator coming from GQI to a DOM query.

1. Implement the `IGQIOptimizableDataSource` interface.
1. Store the sort operator, from the `Optimize` method.
1. Add a mapping between the `GQIColumns` and the correct Exposer.
1. Implement the `IGQIOnPrepareFetch` life cycle hook, to optimize the sort.