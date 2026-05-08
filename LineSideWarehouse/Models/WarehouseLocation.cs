using System;

namespace LineSideWarehouse.Models
{
    /// <summary>
    /// 仓库位模型 - 表示线边仓库的一个存储位置
    /// </summary>
    public class WarehouseLocation : ObservableObject
    {
        private string _locationId = string.Empty;
        private string _locationName = string.Empty;
        private string _materialCode = string.Empty;
        private string _materialName = string.Empty;
        private int _currentQuantity;
        private int _maxCapacity;
        private LocationStatus _status;
        private string? _remark;
        private DateTime _lastUpdateTime;

        public WarehouseLocation()
        {
            LocationId = Guid.NewGuid().ToString("N").Substring(0, 8);
            Status = LocationStatus.Available;
            LastUpdateTime = DateTime.Now;
        }

        /// <summary>
        /// 库位编号
        /// </summary>
        public string LocationId
        {
            get => _locationId;
            set => SetProperty(ref _locationId, value);
        }

        /// <summary>
        /// 库位名称（如：A-01-01）
        /// </summary>
        public string LocationName
        {
            get => _locationName;
            set => SetProperty(ref _locationName, value);
        }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string MaterialCode
        {
            get => _materialCode;
            set
            {
                SetProperty(ref _materialCode, value);
                UpdateStatus();
            }
        }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string MaterialName
        {
            get => _materialName;
            set => SetProperty(ref _materialName, value);
        }

        /// <summary>
        /// 当前数量
        /// </summary>
        public int CurrentQuantity
        {
            get => _currentQuantity;
            set
            {
                SetProperty(ref _currentQuantity, value);
                UpdateStatus();
                LastUpdateTime = DateTime.Now;
            }
        }

        /// <summary>
        /// 最大容量
        /// </summary>
        public int MaxCapacity
        {
            get => _maxCapacity;
            set
            {
                SetProperty(ref _maxCapacity, value);
                UpdateStatus();
            }
        }

        /// <summary>
        /// 库位状态
        /// </summary>
        public LocationStatus Status
        {
            get => _status;
            set => SetProperty(ref _status, value);
        }

        /// <summary>
        /// 备注信息
        /// </summary>
        public string? Remark
        {
            get => _remark;
            set => SetProperty(ref _remark, value);
        }

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime LastUpdateTime
        {
            get => _lastUpdateTime;
            set => SetProperty(ref _lastUpdateTime, value);
        }

        /// <summary>
        /// 使用率百分比
        /// </summary>
        public double UsagePercentage => MaxCapacity > 0 ? (double)CurrentQuantity / MaxCapacity * 100 : 0;

        /// <summary>
        /// 是否空闲
        /// </summary>
        public bool IsAvailable => Status == LocationStatus.Available;

        /// <summary>
        /// 更新库位状态
        /// </summary>
        private void UpdateStatus()
        {
            if (string.IsNullOrEmpty(MaterialCode))
            {
                Status = LocationStatus.Available;
            }
            else if (CurrentQuantity >= MaxCapacity && MaxCapacity > 0)
            {
                Status = LocationStatus.Full;
            }
            else if (CurrentQuantity > 0)
            {
                Status = LocationStatus.Partial;
            }
            else
            {
                Status = LocationStatus.Reserved;
            }
        }

        /// <summary>
        /// 入库操作
        /// </summary>
        public bool Inbound(int quantity, string materialCode, string materialName)
        {
            if (quantity <= 0) return false;
            if (CurrentQuantity + quantity > MaxCapacity) return false;

            MaterialCode = materialCode;
            MaterialName = materialName;
            CurrentQuantity += quantity;
            
            return true;
        }

        /// <summary>
        /// 出库操作
        /// </summary>
        public bool Outbound(int quantity)
        {
            if (quantity <= 0) return false;
            if (CurrentQuantity < quantity) return false;

            CurrentQuantity -= quantity;
            
            if (CurrentQuantity == 0)
            {
                MaterialCode = string.Empty;
                MaterialName = string.Empty;
            }
            
            return true;
        }
    }

    /// <summary>
    /// 库位状态枚举
    /// </summary>
    public enum LocationStatus
    {
        /// <summary>
        /// 空闲
        /// </summary>
        Available = 0,
        
        /// <summary>
        /// 部分占用
        /// </summary>
        Partial = 1,
        
        /// <summary>
        /// 已满
        /// </summary>
        Full = 2,
        
        /// <summary>
        /// 已预留
        /// </summary>
        Reserved = 3,
        
        /// <summary>
        /// 维护中
        /// </summary>
        Maintenance = 4
    }
}
