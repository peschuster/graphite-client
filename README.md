# graphite-client

Windows (.NET) library and tools for feeding data into [Graphite](http://readthedocs.org/docs/graphite/en/latest/overview.html "Graphite is an enterprise-scale monitoring tool") and [statsD](https://github.com/etsy/statsd "StatsD - a network daemon for aggregating statistics").

## Current status

- Base library (Graphite.dll)
- Monitoring service for [PerformanceCounters](http://www.codeproject.com/Articles/8590/An-Introduction-To-Performance-Counters) (PerfCounterMonitor.exe)
- Basic instrumentation of ASP.NET MVC apps (inspired by MiniProfiler)
- Instrumentation of WCF services
- Sending stats from inside SQL Server using TSQL / a stored procedure

## TODO

- Instrumentation of ASP.NET MVC applications
- Montioring log files
- ...

## Building...

How to build:
 
1. Go to `\build` directory
2. Execute `go.bat`