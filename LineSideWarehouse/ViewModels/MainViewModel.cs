using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LineSideWarehouse.Models;
using LineSideWarehouse.Services;
using System.Linq;

namespace LineSideWarehouse.ViewModels
{
    /// <summary>
    /// 主窗口 ViewModel
    /// </summary>
    public partial class MainViewModel : ObservableObject
    {
        private readonly IWarehouseService _warehouseService;

        [ObservableProperty]
        private ObservableCollection<WarehouseLocation> _locations = new();

        [ObservableProperty]
        private WarehouseLocation? _selectedLocation;

        [ObservableProperty]
        private string _searchText = string.Empty;

        [ObservableProperty]
        private LocationStatus? _filterStatus;

        [ObservableProperty]
        private int _totalCount;

        [ObservableProperty]
        private int _availableCount;

        [ObservableProperty]
        private int _occupiedCount;

        [ObservableProperty]
        private bool _isInboundDialogOpen;

        [ObservableProperty]
        private bool _isOutboundDialogOpen;

        [ObservableProperty]
        private bool _isEditDialogOpen;

        [ObservableProperty]
        private int _inboundQuantity;

        [ObservableProperty]
        private string _inboundMaterialCode = string.Empty;

        [ObservableProperty]
        private string _inboundMaterialName = string.Empty;

        [ObservableProperty]
        private int _outboundQuantity;

        public MainViewModel(IWarehouseService warehouseService)
        {
            _warehouseService = warehouseService;
            LoadLocations();
            UpdateStatistics();
        }

        /// <summary>
        /// 加载库位数据
        /// </summary>
        private void LoadLocations()
        {
            Locations = new ObservableCollection<WarehouseLocation>(_warehouseService.GetAllLocations());
            UpdateStatistics();
        }

        /// <summary>
        /// 更新统计数据
        /// </summary>
        private void UpdateStatistics()
        {
            TotalCount = Locations.Count;
            AvailableCount = Locations.Count(l => l.Status == LocationStatus.Available);
            OccupiedCount = Locations.Count(l => l.Status != LocationStatus.Available);
        }

        /// <summary>
        /// 搜索和过滤库位
        /// </summary>
        partial void OnSearchTextChanged(string value)
        {
            ApplyFilter();
        }

        partial void OnFilterStatusChanged(LocationStatus? value)
        {
            ApplyFilter();
        }

        private void ApplyFilter()
        {
            var allLocations = _warehouseService.GetAllLocations();
            
            var filtered = allLocations.Where(l =>
            {
                // 搜索条件
                bool matchSearch = string.IsNullOrEmpty(SearchText) ||
                    l.LocationName.Contains(SearchText, System.StringComparison.OrdinalIgnoreCase) ||
                    l.MaterialCode.Contains(SearchText, System.StringComparison.OrdinalIgnoreCase) ||
                    l.MaterialName.Contains(SearchText, System.StringComparison.OrdinalIgnoreCase);

                // 状态过滤
                bool matchStatus = !FilterStatus.HasValue || l.Status == FilterStatus.Value;

                return matchSearch && matchStatus;
            }).ToList();

            Locations = new ObservableCollection<WarehouseLocation>(filtered);
        }

        /// <summary>
        /// 打开入库对话框
        /// </summary>
        [RelayCommand]
        private void OpenInboundDialog()
        {
            if (SelectedLocation == null || !SelectedLocation.IsAvailable) return;

            InboundQuantity = 0;
            InboundMaterialCode = string.Empty;
            InboundMaterialName = string.Empty;
            IsInboundDialogOpen = true;
        }

        /// <summary>
        /// 执行入库操作
        /// </summary>
        [RelayCommand]
        private void ExecuteInbound()
        {
            if (SelectedLocation == null) return;
            if (InboundQuantity <= 0) return;
            if (string.IsNullOrEmpty(InboundMaterialCode)) return;

            bool success = SelectedLocation.Inbound(InboundQuantity, InboundMaterialCode, InboundMaterialName);
            
            if (success)
            {
                _warehouseService.UpdateLocation(SelectedLocation);
                ApplyFilter();
                UpdateStatistics();
                IsInboundDialogOpen = false;
            }
        }

        /// <summary>
        /// 取消入库
        /// </summary>
        [RelayCommand]
        private void CancelInbound()
        {
            IsInboundDialogOpen = false;
        }

        /// <summary>
        /// 打开出库对话框
        /// </summary>
        [RelayCommand]
        private void OpenOutboundDialog()
        {
            if (SelectedLocation == null || SelectedLocation.CurrentQuantity <= 0) return;

            OutboundQuantity = Math.Min(SelectedLocation.CurrentQuantity, 10);
            IsOutboundDialogOpen = true;
        }

        /// <summary>
        /// 执行出库操作
        /// </summary>
        [RelayCommand]
        private void ExecuteOutbound()
        {
            if (SelectedLocation == null) return;
            if (OutboundQuantity <= 0) return;
            if (OutboundQuantity > SelectedLocation.CurrentQuantity) return;

            bool success = SelectedLocation.Outbound(OutboundQuantity);
            
            if (success)
            {
                _warehouseService.UpdateLocation(SelectedLocation);
                ApplyFilter();
                UpdateStatistics();
                IsOutboundDialogOpen = false;
            }
        }

        /// <summary>
        /// 取消出库
        /// </summary>
        [RelayCommand]
        private void CancelOutbound()
        {
            IsOutboundDialogOpen = false;
        }

        /// <summary>
        /// 打开编辑对话框
        /// </summary>
        [RelayCommand]
        private void OpenEditDialog()
        {
            if (SelectedLocation == null) return;
            IsEditDialogOpen = true;
        }

        /// <summary>
        /// 保存编辑
        /// </summary>
        [RelayCommand]
        private void SaveEdit()
        {
            if (SelectedLocation == null) return;
            
            _warehouseService.UpdateLocation(SelectedLocation);
            ApplyFilter();
            UpdateStatistics();
            IsEditDialogOpen = false;
        }

        /// <summary>
        /// 取消编辑
        /// </summary>
        [RelayCommand]
        private void CancelEdit()
        {
            IsEditDialogOpen = false;
        }

        /// <summary>
        /// 添加新库位
        /// </summary>
        [RelayCommand]
        private void AddNewLocation()
        {
            var newLocation = new WarehouseLocation
            {
                LocationName = $"NEW-{Locations.Count + 1:D3}",
                MaxCapacity = 100
            };
            
            _warehouseService.AddLocation(newLocation);
            LoadLocations();
        }

        /// <summary>
        /// 删除选中库位
        /// </summary>
        [RelayCommand]
        private void DeleteLocation()
        {
            if (SelectedLocation == null) return;
            
            _warehouseService.DeleteLocation(SelectedLocation.LocationId);
            LoadLocations();
            SelectedLocation = null;
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        [RelayCommand]
        private void Refresh()
        {
            LoadLocations();
        }
    }
}
