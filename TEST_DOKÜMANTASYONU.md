# 🧪 API Test Dokümantasyonu - Bruno/Postman

Bu dokümantasyon, API'yi Bruno veya Postman gibi araçlarla test etmek için hazırlanmıştır.

## 📋 İçindekiler

- [Hazırlık](#-hazırlık)
- [Base URL](#-base-url)
- [Test Senaryoları](#-test-senaryoları)
  - [Users Endpoints](#1-users-endpoints)
  - [Products Endpoints](#2-products-endpoints)
  - [Orders Endpoints](#3-orders-endpoints)
  - [Minimal API Endpoints](#4-minimal-api-endpoints)
  - [Error Handling Testleri](#5-error-handling-testleri)

---

## 🚀 Hazırlık

1. Projeyi çalıştırın:
```bash
cd SoftwareArchitecture.Api
dotnet run
```

2. API'nin çalıştığını kontrol edin:
   - Swagger UI: `http://localhost:5135` veya `https://localhost:7059`
   - Health Check: `GET http://localhost:5135/api/health`

---

## 🌐 Base URL

- **HTTP**: `http://localhost:5135`
- **HTTPS**: `https://localhost:7059`

---

## 📝 Test Senaryoları

### 1. Users Endpoints

#### 1.1. Tüm Kullanıcıları Getir

**Request:**
```
GET http://localhost:5135/api/users
```

**Expected Response (200 OK):**
```json
{
  "success": true,
  "message": "Users fetched successfully",
  "data": [
    {
      "id": 1,
      "name": "Ahmet Yılmaz"
    }
  ]
}
```

---

#### 1.2. ID'ye Göre Kullanıcı Getir

**Request:**
```
GET http://localhost:5135/api/users/1
```

**Expected Response (200 OK):**
```json
{
  "success": true,
  "message": "User fetched successfully",
  "data": {
    "id": 1,
    "name": "Ahmet Yılmaz"
  }
}
```

**Expected Response (404 Not Found):**
```json
{
  "success": false,
  "message": "User not found",
  "data": null
}
```

---

#### 1.3. Yeni Kullanıcı Oluştur

**Request:**
```
POST http://localhost:5135/api/users
Content-Type: application/json

{
  "name": "Mehmet Demir"
}
```

**Expected Response (201 Created):**
```json
{
  "success": true,
  "message": "User created successfully",
  "data": {
    "id": 2,
    "name": "Mehmet Demir"
  }
}
```

**Validation Error Test (400 Bad Request):**
```
POST http://localhost:5135/api/users
Content-Type: application/json

{
  "name": ""
}
```

**Expected Response (400 Bad Request):**
```json
{
  "success": false,
  "message": "Validation failed: Name is required",
  "data": null
}
```

---

#### 1.4. Kullanıcı Güncelle

**Request:**
```
PUT http://localhost:5135/api/users/1
Content-Type: application/json

{
  "name": "Ahmet Yılmaz (Güncellendi)"
}
```

**Expected Response (200 OK):**
```json
{
  "success": true,
  "message": "User updated successfully",
  "data": {
    "id": 1,
    "name": "Ahmet Yılmaz (Güncellendi)"
  }
}
```

---

#### 1.5. Kullanıcı Sil

**Request:**
```
DELETE http://localhost:5135/api/users/1
```

**Expected Response (204 No Content):**
```
(No body)
```

---

### 2. Products Endpoints

#### 2.1. Tüm Ürünleri Getir

**Request:**
```
GET http://localhost:5135/api/products
```

**Expected Response (200 OK):**
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

---

#### 2.2. ID'ye Göre Ürün Getir

**Request:**
```
GET http://localhost:5135/api/products/1
```

**Expected Response (200 OK):**
```json
{
  "success": true,
  "message": "Product fetched successfully",
  "data": {
    "id": 1,
    "name": "Laptop",
    "price": 15000.00,
    "createdAt": "2024-01-01T10:00:00Z"
  }
}
```

---

#### 2.3. Yeni Ürün Oluştur

**Request:**
```
POST http://localhost:5135/api/products
Content-Type: application/json

{
  "name": "Mouse",
  "price": 250.50
}
```

**Expected Response (201 Created):**
```json
{
  "success": true,
  "message": "Product created successfully",
  "data": {
    "id": 2,
    "name": "Mouse",
    "price": 250.50,
    "createdAt": "2024-01-01T10:00:00Z"
  }
}
```

**Validation Error Test (400 Bad Request):**
```
POST http://localhost:5135/api/products
Content-Type: application/json

{
  "name": "",
  "price": -10
}
```

**Expected Response (400 Bad Request):**
```json
{
  "success": false,
  "message": "Validation failed: Name is required, Price must be between 0.01 and 999999.99",
  "data": null
}
```

---

#### 2.4. Ürün Güncelle

**Request:**
```
PUT http://localhost:5135/api/products/1
Content-Type: application/json

{
  "name": "Gaming Laptop",
  "price": 20000.00
}
```

**Expected Response (200 OK):**
```json
{
  "success": true,
  "message": "Product updated successfully",
  "data": {
    "id": 1,
    "name": "Gaming Laptop",
    "price": 20000.00,
    "createdAt": "2024-01-01T10:00:00Z"
  }
}
```

---

#### 2.5. Ürün Sil

**Request:**
```
DELETE http://localhost:5135/api/products/1
```

**Expected Response (204 No Content):**
```
(No body)
```

---

### 3. Orders Endpoints

#### 3.1. Tüm Siparişleri Getir

**Request:**
```
GET http://localhost:5135/api/orders
```

**Expected Response (200 OK):**
```json
{
  "success": true,
  "message": "Orders fetched successfully",
  "data": [
    {
      "id": 1,
      "userId": 1,
      "createdAt": "2024-01-01T10:00:00Z",
      "items": [
        {
          "productId": 1,
          "quantity": 2
        }
      ]
    }
  ]
}
```

---

#### 3.2. ID'ye Göre Sipariş Getir

**Request:**
```
GET http://localhost:5135/api/orders/1
```

**Expected Response (200 OK):**
```json
{
  "success": true,
  "message": "Order fetched successfully",
  "data": {
    "id": 1,
    "userId": 1,
    "createdAt": "2024-01-01T10:00:00Z",
    "items": [
      {
        "productId": 1,
        "quantity": 2
      }
    ]
  }
}
```

---

#### 3.3. Yeni Sipariş Oluştur

**ÖNEMLİ:** Önce en az 1 User ve 1 Product oluşturmanız gerekiyor!

**Request:**
```
POST http://localhost:5135/api/orders
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

**Expected Response (201 Created):**
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

**Validation Error Test (400 Bad Request):**
```
POST http://localhost:5135/api/orders
Content-Type: application/json

{
  "userId": 0,
  "items": []
}
```

**Expected Response (400 Bad Request):**
```json
{
  "success": false,
  "message": "Validation failed: UserId must be greater than 0, At least one item is required",
  "data": null
}
```

---

#### 3.4. Sipariş Güncelle

**Request:**
```
PUT http://localhost:5135/api/orders/1
Content-Type: application/json

{
  "userId": 1,
  "items": [
    {
      "productId": 1,
      "quantity": 5
    }
  ]
}
```

**Expected Response (200 OK):**
```json
{
  "success": true,
  "message": "Order updated successfully",
  "data": {
    "id": 1,
    "userId": 1,
    "createdAt": "2024-01-01T10:00:00Z",
    "items": [
      {
        "productId": 1,
        "quantity": 5
      }
    ]
  }
}
```

---

#### 3.5. Sipariş Sil

**Request:**
```
DELETE http://localhost:5135/api/orders/1
```

**Expected Response (204 No Content):**
```
(No body)
```

---

### 4. Minimal API Endpoints

#### 4.1. Products - Minimal API

**GET Tüm Ürünler:**
```
GET http://localhost:5135/api/minimal/products
```

**GET ID'ye Göre:**
```
GET http://localhost:5135/api/minimal/products/1
```

**POST Yeni Ürün:**
```
POST http://localhost:5135/api/minimal/products
Content-Type: application/json

{
  "name": "Keyboard",
  "price": 500.00
}
```

**PUT Güncelle:**
```
PUT http://localhost:5135/api/minimal/products/1
Content-Type: application/json

{
  "name": "Mechanical Keyboard",
  "price": 750.00
}
```

**DELETE Sil:**
```
DELETE http://localhost:5135/api/minimal/products/1
```

---

#### 4.2. Orders - Minimal API

**GET Tüm Siparişler:**
```
GET http://localhost:5135/api/minimal/orders
```

**GET ID'ye Göre:**
```
GET http://localhost:5135/api/minimal/orders/1
```

**POST Yeni Sipariş:**
```
POST http://localhost:5135/api/minimal/orders
Content-Type: application/json

{
  "userId": 1,
  "items": [
    {
      "productId": 1,
      "quantity": 3
    }
  ]
}
```

**PUT Güncelle:**
```
PUT http://localhost:5135/api/minimal/orders/1
Content-Type: application/json

{
  "userId": 1,
  "items": [
    {
      "productId": 1,
      "quantity": 10
    }
  ]
}
```

**DELETE Sil:**
```
DELETE http://localhost:5135/api/minimal/orders/1
```

---

### 5. Error Handling Testleri

#### 5.1. 404 Not Found Testi

**Request:**
```
GET http://localhost:5135/api/users/99999
```

**Expected Response (404 Not Found):**
```json
{
  "success": false,
  "message": "User not found",
  "data": null
}
```

---

#### 5.2. 400 Bad Request - Validation Testi

**Request:**
```
POST http://localhost:5135/api/products
Content-Type: application/json

{
  "name": "A",
  "price": -5
}
```

**Expected Response (400 Bad Request):**
```json
{
  "success": false,
  "message": "Validation failed: Name must be between 2 and 200 characters, Price must be between 0.01 and 999999.99",
  "data": null
}
```

---

#### 5.3. 404 Not Found - Endpoint Bulunamadı

**Request:**
```
GET http://localhost:5135/api/nonexistent
```

**Expected Response (404 Not Found):**
```json
{
  "success": false,
  "message": "Endpoint not found",
  "data": null
}
```

---

## 📊 Test Senaryosu Sırası (Önerilen)

Testleri aşağıdaki sırayla yapmanız önerilir:

1. ✅ **Health Check**
   - `GET /api/health`

2. ✅ **Users CRUD**
   - POST → User oluştur (ID: 1)
   - GET → Tüm users
   - GET → User ID: 1
   - PUT → User ID: 1 güncelle
   - DELETE → User ID: 1 sil

3. ✅ **Products CRUD**
   - POST → Product oluştur (ID: 1, 2)
   - GET → Tüm products
   - GET → Product ID: 1
   - PUT → Product ID: 1 güncelle
   - DELETE → Product ID: 1 sil (AMA ID: 2'yi silme, Order için gerekli!)

4. ✅ **Orders CRUD**
   - POST → Order oluştur (User ID: 1, Product ID: 2 kullan)
   - GET → Tüm orders
   - GET → Order ID: 1
   - PUT → Order ID: 1 güncelle
   - DELETE → Order ID: 1 sil

5. ✅ **Minimal API Testleri**
   - Products Minimal API (GET, POST, PUT, DELETE)
   - Orders Minimal API (GET, POST, PUT, DELETE)

6. ✅ **Error Handling Testleri**
   - 404 Not Found
   - 400 Bad Request (Validation)
   - Geçersiz endpoint

---

## 🔍 Bruno/Postman Collection İçin Notlar

### Bruno için:
- Her endpoint'i ayrı bir dosya olarak kaydedebilirsiniz
- Environment variable olarak `baseUrl` tanımlayın: `http://localhost:5135`

### Postman için:
- Collection oluşturun: "Software Architecture API"
- Environment oluşturun: `baseUrl = http://localhost:5135`
- Her endpoint için ayrı request oluşturun

---

## ✅ Başarı Kriterleri

Her test için kontrol edilmesi gerekenler:

1. ✅ **Status Code** doğru mu? (200, 201, 204, 400, 404)
2. ✅ **Response Format** standart mı? (`success`, `message`, `data`)
3. ✅ **Data** doğru mu?
4. ✅ **Validation** çalışıyor mu?
5. ✅ **Error Messages** anlamlı mı?

---

## 🐛 Sorun Giderme

### API çalışmıyor:
```bash
cd SoftwareArchitecture.Api
dotnet run
```

### Veritabanı hatası:
```bash
cd SoftwareArchitecture.Api
dotnet ef database update
```

### Port değişikliği:
`launchSettings.json` dosyasından port numarasını kontrol edin.

---

**İyi testler! 🚀**
