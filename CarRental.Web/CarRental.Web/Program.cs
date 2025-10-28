using CarRental.Web.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<CarRentalDbContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
	app.UseHsts();
}

app.UseStaticFiles();

app.UseRouting();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();


//CarRental.Web /
//│
//├── Controllers /
//│   ├── HomeController.cs        // Главная страница, навигация
//│   ├── CarsController.cs        // Управление каталогом машин
//│   ├── OrdersController.cs      // Управление заказами
//│   ├── AccountController.cs     // Авторизация и регистрация
//│   └── AdminController.cs       // Админка (панель управления)
//│
//├── Models /
//│   ├── Car.cs                   // Модель машины
//│   ├── Customer.cs              // Модель клиента
//│   ├── Order.cs                 // Модель заказа
//│   └── CarRentalDbContext.cs    // Контекст БД (EF Core)
//│
//├── Views /
//│   ├── Home /
//│   │   └── Index.cshtml         // Главная страница
//│   ├── Cars /
//│   │   ├── Index.cshtml         // Список машин (каталог)
//│   │   ├── Details.cshtml       // Подробности машины
//│   │   ├── Create.cshtml        // Добавление (в админке)
//│   │   ├── Edit.cshtml          // Редактирование (в админке)
//│   │   └── Delete.cshtml        // Удаление (в админке)
//│   ├── Orders /
//│   │   ├── Index.cshtml         // Просмотр заказов пользователя
//│   │   ├── Create.cshtml        // Оформление заказа
//│   │   └── Confirm.cshtml       // Подтверждение заказа
//│   ├── Account/
//│   │   ├── Login.cshtml
//│   │   └── Register.cshtml
//│   └── Shared/
//│       ├── _Layout.cshtml       // Общий шаблон (меню, футер)
//│       └── _ValidationScriptsPartial.cshtml
//│
//├── wwwroot/
//│   ├── css/
//│   ├── js/
//│   ├── images/
//│   └── uploads/                 // Загруженные фото машин
//│
//├── appsettings.json             // Подключение к БД
//├── Program.cs                   // Конфигурация приложения
//└── CarRental.Web.csproj