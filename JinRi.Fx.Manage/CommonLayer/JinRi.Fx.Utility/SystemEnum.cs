using JinRi.Fx.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JinRi.Fx.Utility
{
    public enum EntityStatus
    {
        [EnumTitle("启用")]
        Enabled=0,

        [EnumTitle("禁用")]
        Disabled=1
    }

    public enum MetricType
    {
        Meters,
        Histograms
    }

    public enum IntervalUnit
    {
        Minute,
        Hour,
        Day
    }

    public enum AggregationWay
    {
        SUM,
        COUNT,
        AVG,
        MAX,
        MIN
    }

    public enum GroupBy
    {
        NotSet,
        AppID,
        HostIP
    }
}
