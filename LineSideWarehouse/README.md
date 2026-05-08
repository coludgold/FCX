# 线边仓库位管理系统 (Line Side Warehouse Location Management System)

## 项目简介

这是一个基于 WPF (.NET 6) 开发的线边仓库位管理系统，用于管理生产线边的物料存储位置。系统采用 MVVM 架构模式，使用 CommunityToolkit.Mvvm 库实现数据绑定和命令处理。

## 功能特性

### 核心功能
- **库位管理**: 创建、编辑、删除仓库存储位置
- **入库操作**: 将物料存入选定库位
- **出库操作**: 从库位取出物料
- **实时状态监控**: 显示库位的使用状态（空闲、部分占用、已满等）
- **搜索过滤**: 按库位名称、物料编码、物料名称搜索，按状态过滤
- **可视化展示**: 使用颜色编码和进度条直观显示库位使用情况

### 界面特点
- 现代化 UI 设计，采用 Material Design 风格配色
- 状态指示器：绿色（空闲）、橙色（部分占用）、红色（已满）、蓝色（已预留）、灰色（维护中）
- 库存进度条：根据使用率动态显示颜色变化
- 统计面板：实时显示总库位数、空闲数、占用数

## 技术栈

- **框架**: WPF (.NET 6)
- **MVVM 库**: CommunityToolkit.Mvvm 8.2.2
- **JSON 处理**: Newtonsoft.Json 13.0.3
- **架构模式**: MVVM (Model-View-ViewModel)

## 项目结构

```
LineSideWarehouse/
├── Models/                 # 数据模型
│   └── WarehouseLocation.cs    # 库位模型
├── ViewModels/             # ViewModel 层
│   └── MainViewModel.cs        # 主窗口 ViewModel
├── Views/                  # 视图层
│   ├── MainWindow.xaml         # 主窗口 UI
│   └── MainWindow.xaml.cs      # 主窗口代码后台
├── Services/               # 服务层
│   └── WarehouseService.cs     # 仓库服务接口和实现
├── Converters/             # 值转换器
│   └── StatusConverters.cs     # 状态到颜色/文本的转换器
├── App.xaml                # 应用程序定义
├── App.xaml.cs             # 应用程序代码
└── LineSideWarehouse.csproj    # 项目文件
```

## 主要类说明

### Models
- **WarehouseLocation**: 库位模型，包含库位 ID、名称、物料信息、数量、容量、状态等属性
- **LocationStatus**: 库位状态枚举（Available, Partial, Full, Reserved, Maintenance）

### ViewModels
- **MainViewModel**: 主窗口 ViewModel，实现所有业务逻辑和命令
  - 数据加载和刷新
  - 搜索和过滤功能
  - 入库/出库/编辑操作
  - 统计数据计算

### Services
- **IWarehouseService**: 仓库服务接口
- **InMemoryWarehouseService**: 内存实现（演示用），包含示例数据

### Converters
- **StatusToColorConverter**: 状态转颜色
- **StatusToTextConverter**: 状态转中文文本
- **UsageToColorConverter**: 使用率转颜色
- **InverseBooleanConverter**: 布尔值取反

## 使用方法

### 环境要求
- Windows 10/11
- .NET 6 SDK 或更高版本
- Visual Studio 2022（推荐）或 VS Code

### 编译运行

```bash
# 进入项目目录
cd LineSideWarehouse

# 还原 NuGet 包
dotnet restore

# 编译项目
dotnet build

# 运行程序
dotnet run
```

### 操作说明

1. **查看库位**: 主界面以表格形式显示所有库位及其状态
2. **搜索库位**: 在顶部搜索框输入关键词（库位名称、物料编码、物料名称）
3. **过滤状态**: 使用状态下拉框筛选特定状态的库位
4. **入库操作**: 
   - 选择一个空闲库位
   - 点击"📥 入库"按钮
   - 填写物料信息和数量
   - 确认入库
5. **出库操作**:
   - 选择一个有库存的库位
   - 点击"📤 出库"按钮
   - 填写出库数量
   - 确认出库
6. **编辑库位**: 选择库位后点击"✏️ 编辑"修改库位信息
7. **新建库位**: 点击"➕ 新建库位"添加新库位
8. **删除库位**: 选择库位后点击"🗑️ 删除"

## 扩展建议

当前版本使用内存存储，实际生产环境中可以扩展：

1. **数据库持久化**: 集成 SQLite、SQL Server 或其他数据库
2. **条码扫描**: 添加条码扫描功能快速录入物料
3. **权限管理**: 添加用户登录和权限控制
4. **报表功能**: 生成库存报表、出入库记录
5. **API 接口**: 提供 REST API 与其他系统集成
6. **预警功能**: 库存不足或超储时发出预警

## 许可证

MIT License

## 联系方式

如有问题或建议，欢迎联系开发团队。
