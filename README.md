# graphite-client

Windows (.NET) library and tools for feeding data into [Graphite](http://readthedocs.org/docs/graphite/en/latest/overview.html "Graphite is an enterprise-scale monitoring tool") and [statsD](https://github.com/etsy/statsd "StatsD - a network daemon for aggregating statistics").

## Current status

- Base library (Graphite.dll)
- Monitoring service for [PerformanceCounters](http://www.codeproject.com/Articles/8590/An-Introduction-To-Performance-Counters) (PerfCounterMonitor.exe)
- Basic instrumentation of ASP.NET MVC apps (inspired by MiniProfiler, Graphite.Mvc.dll)
- Instrumentation of WCF services (Graphite.Wcf.dll)
- Sending stats from inside SQL Server using TSQL / a stored procedure (Graphite.TSql.dll)
- MSBuild task for sending stats to Graphite and StatsD  (MSBuild.Graphite.dll)
- [ELMAH](http://code.google.com/p/elmah/) [error filter](http://code.google.com/p/elmah/wiki/ErrorFiltering) for logging all exception to Graphite or StatsD (Graphite.Elmah.dll)

## Published NuGet packages

 - [Graphite](http://nuget.org/packages/Graphite)
 - [Graphite.Wcf](http://nuget.org/packages/Graphite.Wcf)
 - [Graphite.Elmah](http://nuget.org/packages/Graphite.Elmah)

## Quickstart

1. Install NugGet package [Graphite](http://nuget.org/packages/Graphite) (for standalone applications) or [Graphite.Wcf](http://nuget.org/packages/Graphite.Wcf) for WCF applications or Reference Graphite.Mvc.dll (currently not on NuGet) for ASP.NET MVC applications.

2. Add configuration for the base Graphite DLL to your App.config/Web.config:

```
<configSections>
  <section name="graphite" type="Graphite.Configuration.GraphiteConfiguration, Graphite" />
</configSections>
<graphite xmlns="http://github.com/peschuster/Graphite/Configuration">
  <!--<graphite address="127.0.0.1" port="2003" transport="Tcp" />-->
  <statsd address="127.0.0.1" port="8125" prefixKey="test" />
</graphite>
```

3. Add a `using` directive for *Graphite*: `using Graphite;`

4. [For standalone apps] Create a profiler instance on application start: `StaticMetricsPipeProvider.Instance.Start();` and stop it on application exit: `StaticMetricsPipeProvider.Instance.Stop();`.

5. Use Graphite in your code: `MetricsPipe.Current.Count("exception");`

## Features/Documentation

### General

The architecture of the *Graphite* system is inspired by [MiniProfiler](http://github.com/SamSaffron/MiniProfiler):

An `IMetricsPipeProvider` manages the current profiler instance. The implementation of this provider differs depending on the context of the application: ASP.NET/Web, WCF and "standalone"/simple exe file.

Accessing the profiler works always the same through `MetricsPipe.Current`

#### Metric Types

There are three different metric types:

 - `counter` - all values in one flush interval are summed up and submitted as is and as "per second" value by *StatsD* to the underlying backend (e.g. *Graphite*) (only for *StatsD*)
 - `timing` - all values in one flush interval are aggregated by several statistical operations (mean, upper, lower, ...) (only for *StatsD*)
 - `gauge` - for *StatsD* the latest, reported value is taken, for *Graphite* the value is reported as is to the *Graphite* server (for *StatsD* and *Graphite*)

### Code instrumentation
> Graphite.dll

Reporting metrics to *Graphite* or *StatsD* can be done by calling one of the extension methods on `MetricsPipe.Current`. Therefore a `using` directive for the `Graphite` namespace is required:

    using Graphite;

The available extension methods correspond to the metric types described in *General*.

**StatsD**:

 - `void Timing(this MetricsPipe profiler, string key, int value)` - for direct submission of *timing* values
 - `IDisposable Step(this MetricsPipe profiler, string key)` - for profiling code segements
 - `void Count(this MetricsPipe profiler, string key, int value = 1, float sampling = 1)` - for reporting *counter* values
 - `void Gauge(this MetricsPipe profiler, string key, int value)` - for reporting *gauges*


**Graphite**:

 - `void Raw(this MetricsPipe profiler, string key, int value)` - for directly reporting values to Graphite

An extension method can be called like this:

    MetricsPipe.Current.Count("exception");

The `Step` method is a special method which measures the time till `Dispose()` is called on the returned `IDisposable`. This can be done best with a `using` statement:

    using (MetricsPipe.Current.Step("duration"))
    {
	    // Do some work here...
	}

The strength of extension methods is that they also work without throwing a `NullReferenceException`, when `MetricsPipe.Current` is `null` - i.e. not initialized.

#### Standalone Applications

The `MetricsPipe` can be used in standalone applications (e.g. console or windows applications) after starting the profiler with the following line of code:

    StaticMetricsPipeProvider.Instance.Start();

Everything is disposed and cleaned up after calling

    StaticMetricsPipeProvider.Instance.Stop();

### System Metrics
> PerfCounterMonitor.exe (Graphite.System)

Besides profiling and instrumenting your code manually you can report various system parameters to *StatsD* and *Graphite*, too. This can be accomplished with the `Graphite.System` block, respectively `PerfCounterMonitor.exe`.

`Graphite.System` enables reporting values from the following sources:

 - Performance counters
 - Event log
 - IIS Application Pools

See the complete [documentation](http://github.com/peschuster/graphite-client/wiki/System-Metrics) for `PerfCounterMonitor.exe`.

**You can download the binaries of *Graphite.System* here:** [Graphite.System v1.0.0](https://github.com/peschuster/graphite-client/releases/tag/Graphite.System_1-0-0-0)


### WCF
> Graphite.Wcf.dll

WCF services can be instrumented for reporting hit counts and execution times. Therefore an attribute needs to be added to the service instance:

    [ServiceBehavior]
    [MetricsPipeBehavior(true, true, fixedRequestTimeKey: null, requestTimePrefix: "service.example", fixedHitCountKey: null, hitCountPrefix: "service.example")]
    public class ExampleGraphiteService
    {
        public void Test()
        {
        }
    }

The attribute has the following parameters:

 - `bool reportRequestTime` - enable reporting of execution time
 - `bool reportHitCount` - enable reporting of hit counts
 - `string fixedRequestTimeKey = null` - fixed metric key for execution time metrics
 - `string requestTimePrefix = null` - prefix key for execution time metrics
 - `string fixedHitCountKey = null` - fixed metric key for hit count metrics
 - `string hitCountPrefix = null` - prefix key for hit count metrics

Either the `fixed...Key` or `...Prefix` parameter must be set for an enabled metric.

On setting the `fixed...Key` parameter, this metric is reported with the specified key as is.

On setting the  `...Prefix` parameter, this metric is reported with a metric key in the following format:

    [prefixKey].[servicename].[operation_name]

### MSBuild Tasks
> MSBuild.Graphite.dll

Reporting metrics from within MSBuild scripts is possible utilizing the tasks in `MSBuild.Graphite.dll`

  	<UsingTask TaskName="MSBuild.Graphite.Tasks.Graphite" AssemblyFile=".\MSBuild.Graphite.dll"></UsingTask>
	
	  <Target Name="graphite">
	    <MSBuild.Graphite.Tasks.Graphite
	        Address="192.168.2.100"
	        Transport="Tcp"
	        PrefixKey="stats.events"
	        Key="deploys.test"
	        Value="1" />
	  </Target>

### ELMAH
> Graphite.Elmah.dll

Exceptions captured by [ELMAH](http://code.google.com/p/elmah/) can be reported using an ELMAH error filter defined in `Graphite.Elmah.dll`.

This can be accomplished by the following settings in your `web.config` file:

	<configSections>
	    <section name="graphite.elmah" type="Graphite.Configuration.GraphiteElmahConfiguration, Graphite.Elmah"/>
	</configSections>
	<graphite.elmah
	    xmlns="http://github.com/peschuster/Graphite/Configuration"
	    key="elmah_errors"
	    type="counter"
	    target="statsd" />
	<elmah>
	    <errorFilter>
	        <test
	            xmlns:my="http://schemas.microsoft.com/clr/nsassem/Graphite/Graphite.Elmah">
	            <my:log />
	        </test>
	    </errorFilter>
	</elmah>

On using the NuGet package [`Graphite.Elmah`](http://nuget.org/packages/Graphite.Elmah) all required settings are added to your project automatically during installation.

### SQL Server
> Graphite.TSql.dll

From within SQL Server, metrics can be reported by calling a stored procedure:

    exec sp_graphitesend N'192.168.0.1', 2003, 'stats.events.myserver.test', 1

This stored procedure can be installed using the provided sql script: [sp_graphitesend.sql](http://github.com/peschuster/graphite-client/blob/master/source/Graphite.TSql/sp_graphitesend.sql)

(see also [Sending stats to Graphite from within SQL Server](http://www.peschuster.de/2012/07/sending-stats-to-graphite-from-within-sql-server/))

## TODO

- Instrumentation of ASP.NET MVC applications
- Montioring log files
- ...

## Building...

How to build:
 
1. Go to `\build` directory
2. Execute `go.bat`
