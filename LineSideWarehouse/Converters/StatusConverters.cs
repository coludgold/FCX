using System;
using System.Globalization;
using System.Windows.Data;
using LineSideWarehouse.Models;

namespace LineSideWarehouse.Converters
{
    /// <summary>
    /// 库位状态到颜色的转换器
    /// </summary>
    public class StatusToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is LocationStatus status)
            {
                return status switch
                {
                    LocationStatus.Available => "#4CAF50",      // 绿色 - 空闲
                    LocationStatus.Partial => "#FF9800",        // 橙色 - 部分占用
                    LocationStatus.Full => "#F44336",           // 红色 - 已满
                    LocationStatus.Reserved => "#2196F3",       // 蓝色 - 已预留
                    LocationStatus.Maintenance => "#9E9E9E",    // 灰色 - 维护中
                    _ => "#757575"
                };
            }
            return "#757575";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 库位状态到文本的转换器
    /// </summary>
    public class StatusToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is LocationStatus status)
            {
                return status switch
                {
                    LocationStatus.Available => "空闲",
                    LocationStatus.Partial => "部分占用",
                    LocationStatus.Full => "已满",
                    LocationStatus.Reserved => "已预留",
                    LocationStatus.Maintenance => "维护中",
                    _ => "未知"
                };
            }
            return "未知";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 使用率到颜色的转换器
    /// </summary>
    public class UsageToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double usage)
            {
                if (usage == 0) return "#4CAF50";       // 0% - 绿色
                if (usage < 50) return "#8BC34A";       // <50% - 浅绿
                if (usage < 75) return "#FFEB3B";       // <75% - 黄色
                if (usage < 90) return "#FF9800";       // <90% - 橙色
                return "#F44336";                        // >=90% - 红色
            }
            return "#757575";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 布尔值取反转换器
    /// </summary>
    public class InverseBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return !boolValue;
            }
            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return !boolValue;
            }
            return false;
        }
    }
}
