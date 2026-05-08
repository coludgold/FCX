using System.Collections.Generic;
using LineSideWarehouse.Models;

namespace LineSideWarehouse.Services
{
    /// <summary>
    /// 仓库服务接口
    /// </summary>
    public interface IWarehouseService
    {
        /// <summary>
        /// 获取所有库位
        /// </summary>
        IEnumerable<WarehouseLocation> GetAllLocations();

        /// <summary>
        /// 根据 ID 获取库位
        /// </summary>
        WarehouseLocation? GetLocationById(string locationId);

        /// <summary>
        /// 添加库位
        /// </summary>
        void AddLocation(WarehouseLocation location);

        /// <summary>
        /// 更新库位
        /// </summary>
        void UpdateLocation(WarehouseLocation location);

        /// <summary>
        /// 删除库位
        /// </summary>
        void DeleteLocation(string locationId);
    }

    /// <summary>
    /// 内存仓库服务实现（演示用）
    /// </summary>
    public class InMemoryWarehouseService : IWarehouseService
    {
        private readonly Dictionary<string, WarehouseLocation> _locations = new();

        public InMemoryWarehouseService()
        {
            // 初始化一些示例数据
            InitializeSampleData();
        }

        private void InitializeSampleData()
        {
            var sampleLocations = new List<WarehouseLocation>
            {
                new WarehouseLocation { LocationName = "A-01-01", MaxCapacity = 100 },
                new WarehouseLocation { LocationName = "A-01-02", MaxCapacity = 100 },
                new WarehouseLocation { LocationName = "A-01-03", MaxCapacity = 150 },
                new WarehouseLocation { LocationName = "A-02-01", MaxCapacity = 100 },
                new WarehouseLocation { LocationName = "A-02-02", MaxCapacity = 100 },
                new WarehouseLocation { LocationName = "B-01-01", MaxCapacity = 200 },
                new WarehouseLocation { LocationName = "B-01-02", MaxCapacity = 200 },
                new WarehouseLocation { LocationName = "B-02-01", MaxCapacity = 200 },
                new WarehouseLocation { LocationName = "C-01-01", MaxCapacity = 50 },
                new WarehouseLocation { LocationName = "C-01-02", MaxCapacity = 50 },
            };

            // 模拟一些已占用的库位
            sampleLocations[0].Inbound(80, "MAT-001", "螺丝 M6");
            sampleLocations[1].Inbound(100, "MAT-002", "螺母 M6");
            sampleLocations[2].Inbound(75, "MAT-003", "垫片");
            sampleLocations[5].Inbound(150, "MAT-004", "电缆线");

            foreach (var location in sampleLocations)
            {
                _locations[location.LocationId] = location;
            }
        }

        public IEnumerable<WarehouseLocation> GetAllLocations()
        {
            return _locations.Values.ToList();
        }

        public WarehouseLocation? GetLocationById(string locationId)
        {
            return _locations.TryGetValue(locationId, out var location) ? location : null;
        }

        public void AddLocation(WarehouseLocation location)
        {
            if (!_locations.ContainsKey(location.LocationId))
            {
                _locations[location.LocationId] = location;
            }
        }

        public void UpdateLocation(WarehouseLocation location)
        {
            if (_locations.ContainsKey(location.LocationId))
            {
                _locations[location.LocationId] = location;
            }
        }

        public void DeleteLocation(string locationId)
        {
            if (_locations.ContainsKey(locationId))
            {
                _locations.Remove(locationId);
            }
        }
    }
}
