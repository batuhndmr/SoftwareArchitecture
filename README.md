# Software Architecture - .NET 9 REST API

Bu proje, **.NET 9** kullanılarak geliştirilmiş, **Clean Architecture** prensiplerine uygun bir REST API uygulamasıdır.

## 📋 İçindekiler

- [Teknolojiler](#-teknolojiler)
- [Mimari Yapı](#-mimari-yapı)
- [Kurulum](#-kurulum)
- [API Endpoints](#-api-endpoints)
- [API Response Formatı](#-api-response-formatı)
- [Örnek İstekler](#-örnek-istekler)

## 🚀 Teknolojiler

- **.NET 9**
- **ASP.NET Core Web API**
- **Entity Framework Core 9.0**
- **SQLite** (Veritabanı)
- **Swagger/OpenAPI** (API Dokümantasyonu)
- **Minimal API** (Endpoint'ler için)
- **Clean Architecture** (Katmanlı Mimari)

## 🏗️ Mimari Yapı

Proje **Clean Architecture** prensiplerine uygun olarak 4 katmandan oluşmaktadır:

```
SoftwareArchitecture/
├── SoftwareArchitecture.Api/          # Presentation Layer (Controllers, Middlewares)
├── SoftwareArchitecture.Application/  # Application Layer (Services, DTOs, Interfaces)
├── SoftwareArchitecture.Domain/       # Domain Layer (Entities, Repository Interfaces)
└── SoftwareArchitecture.Infrastructure/ # Infrastructure Layer (Repositories, DbContext)
```

### Katman Açıklamaları

1. **Api Layer**: HTTP isteklerini karşılar, Controller'lar ve Middleware'ler burada bulunur.
2. **Application Layer**: İş mantığı (Business Logic) burada yer alır. Service'ler ve DTO'lar bu katmanda bulunur.
3. **Domain Layer**: Entity'ler ve Repository interface'leri bu katmanda tanımlanır.
4. **Infrastructure Layer**: Veritabanı işlemleri, Repository implementasyonları burada yer alır.

## 📦 Kurulum

### Gereksinimler

- .NET 9 SDK
- Git

### Adımlar

1. Projeyi klonlayın:
```bash
git clone <repository-url>
cd SoftwareArchitecture
```

2. Veritabanı migration'larını uygulayın:
```bash
cd SoftwareArchitecture.Api
dotnet ef database update
```

3. Projeyi çalıştırın:
```bash
dotnet run
```

4. Swagger UI'a erişin:
```
http://localhost:5135
```

## 📌 API Endpoints

### Users Endpoints

| Method | Endpoint | Açıklama |
|--------|----------|----------|
| GET | `/api/users` | Tüm kullanıcıları getirir |
| GET | `/api/users/{id}` | ID'ye göre kullanıcı getirir |
| POST | `/api/users` | Yeni kullanıcı oluşturur |
| PUT | `/api/users/{id}` | Kullanıcı günceller |
| DELETE | `/api/users/{id}` | Kullanıcı siler |

### Products Endpoints

| Method | Endpoint | Açıklama |
|--------|----------|----------|
| GET | `/api/products` | Tüm ürünleri getirir |
| GET | `/api/products/{id}` | ID'ye göre ürün getirir |
| POST | `/api/products` | Yeni ürün oluşturur |
| PUT | `/api/products/{id}` | Ürün günceller |
| DELETE | `/api/products/{id}` | Ürün siler |

### Orders Endpoints

| Method | Endpoint | Açıklama |
|--------|----------|----------|
| GET | `/api/orders` | Tüm siparişleri getirir |
| GET | `/api/orders/{id}` | ID'ye göre sipariş getirir |
| POST | `/api/orders` | Yeni sipariş oluşturur |
| PUT | `/api/orders/{id}` | Sipariş günceller |
| DELETE | `/api/orders/{id}` | Sipariş siler |

### Minimal API Endpoints

| Method | Endpoint | Açıklama |
|--------|----------|----------|
| GET | `/api/minimal/products` | Tüm ürünleri getirir (Minimal API) |
| GET | `/api/minimal/products/{id}` | ID'ye göre ürün getirir (Minimal API) |
| POST | `/api/minimal/products` | Yeni ürün oluşturur (Minimal API) |
| PUT | `/api/minimal/products/{id}` | Ürün günceller (Minimal API) |
| DELETE | `/api/minimal/products/{id}` | Ürün siler (Minimal API) |
| GET | `/api/minimal/orders` | Tüm siparişleri getirir (Minimal API) |
| GET | `/api/minimal/orders/{id}` | ID'ye göre sipariş getirir (Minimal API) |
| POST | `/api/minimal/orders` | Yeni sipariş oluşturur (Minimal API) |
| PUT | `/api/minimal/orders/{id}` | Sipariş günceller (Minimal API) |
| DELETE | `/api/minimal/orders/{id}` | Sipariş siler (Minimal API) |

### Health Check

| Method | Endpoint | Açıklama |
|--------|----------|----------|
| GET | `/api/health` | API sağlık kontrolü |

## 📄 API Response Formatı

Tüm API yanıtları standart bir formatta döner:

```json
{
  "success": true,
  "message": "İşlem başarılı mesajı",
  "data": { ... }
}
```

### Başarılı Yanıt Örneği

```json
{
  "success": true,
  "message": "Products fetched successfully",
  "data": [
    {
      "id": 1,
      "name": "Laptop",
      "price": 15000.00,
      "createdAt": "2024-01-01T10:00:00Z"
    }
  ]
}
```

### Hata Yanıtı Örneği

```json
{
  "success": false,
  "message": "Product not found",
  "data": null
}
```

## 🔧 Örnek İstekler

### User Oluşturma

**Request:**
```http
POST /api/users
Content-Type: application/json

{
  "name": "Ahmet Yılmaz"
}
```

**Response (201 Created):**
```json
{
  "success": true,
  "message": "User created successfully",
  "data": {
    "id": 1,
    "name": "Ahmet Yılmaz"
  }
}
```

### Product Oluşturma

**Request:**
```http
POST /api/products
Content-Type: application/json

{
  "name": "Laptop",
  "price": 15000.00
}
```

**Response (201 Created):**
```json
{
  "success": true,
  "message": "Product created successfully",
  "data": {
    "id": 1,
    "name": "Laptop",
    "price": 15000.00,
    "createdAt": "2024-01-01T10:00:00Z"
  }
}
```

### Order Oluşturma

**Request:**
```http
POST /api/orders
Content-Type: application/json

{
  "userId": 1,
  "items": [
    {
      "productId": 1,
      "quantity": 2
    },
    {
      "productId": 2,
      "quantity": 1
    }
  ]
}
```

**Response (201 Created):**
```json
{
  "success": true,
  "message": "Order created successfully",
  "data": {
    "id": 1,
    "userId": 1,
    "createdAt": "2024-01-01T10:00:00Z",
    "items": [
      {
        "productId": 1,
        "quantity": 2
      },
      {
        "productId": 2,
        "quantity": 1
      }
    ]
  }
}
```

## 🗄️ Veritabanı Yapısı

### Entities

- **User**: Kullanıcı bilgileri
- **Product**: Ürün bilgileri
- **Order**: Sipariş bilgileri
- **OrderItem**: Sipariş kalemleri

### İlişkiler

- User (1) → Orders (N)
- Order (1) → OrderItems (N)
- Product (1) → OrderItems (N)

## 🔒 Status Code'lar

API aşağıdaki HTTP status code'larını kullanır:

- **200 OK**: Başarılı GET, PUT istekleri
- **201 Created**: Başarılı POST istekleri
- **204 No Content**: Başarılı DELETE istekleri
- **400 Bad Request**: Geçersiz istek (validation hataları)
- **404 Not Found**: Kaynak bulunamadı
- **500 Internal Server Error**: Sunucu hatası

## 🛠️ Özellikler

- ✅ Clean Architecture
- ✅ Katmanlı Mimari (Controller, Service, Repository)
- ✅ DTO Kullanımı (Create, Update, Response)
- ✅ Standart API Response Formatı
- ✅ Global Exception Handling
- ✅ Validation (Data Annotations)
- ✅ Logging (Built-in .NET Logging)
- ✅ Swagger/OpenAPI Dokümantasyonu
- ✅ Minimal API Desteği
- ✅ Entity Framework Core Migrations
- ✅ SQLite Veritabanı

## 📝 Notlar

- Tüm entity'ler `BaseEntity` sınıfından türer ve `CreatedAt` ve `UpdatedAt` alanlarına sahiptir.
- API doğrudan entity döndürmez, her zaman DTO kullanır.
- Validation hataları otomatik olarak yakalanır ve standart formatta döner.
- Logging tüm önemli işlemler için kullanılır.

## 👨‍💻 Geliştirici

Bu proje Software Architecture dersi için Asım Batuhan Demir tarafından geliştirilmiştir.
