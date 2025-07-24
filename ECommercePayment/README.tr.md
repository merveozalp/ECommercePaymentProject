# ECommerce Payment API

**.NET 8** ve **Clean Architecture** prensipleri ile geliştirilmiş, harici Balance Management servisi ile entegre olan sağlam, production-ready e-ticaret ödeme API'si.

## 📋 İçindekiler

- [Genel Bakış](#genel-bakış)
- [Mimari](#mimari)
- [Özellikler](#özellikler)
- [API Endpoint'leri](#api-endpointleri)
- [Dayanıklılık Desenleri](#dayanıklılık-desenleri)
- [Başlangıç](#başlangıç)
- [Konfigürasyon](#konfigürasyon)
- [Hata Yönetimi](#hata-yönetimi)
- [Test](#test)
- [Proje Yapısı](#proje-yapısı)
- [Teknoloji Stack](#teknoloji-stack)

## 🎯 Genel Bakış

Bu API, harici Balance Management servisi ile entegrasyon yoluyla yüksek erişilebilirlik ve hata toleransı sağlayarak e-ticaret uygulamaları için ürün yönetimi ve sipariş işleme yetenekleri sunan bir ödeme geçidi olarak hizmet verir.

### Ana Sorumluluklar
- ✅ Ürün kataloğu yönetimi
- ✅ Sipariş oluşturma ve işleme  
- ✅ Fon rezervasyonu ve ödeme tamamlama
- ✅ Harici servis hatalarına karşı sağlam hata yönetimi
- ✅ Swagger ile temiz API dokümantasyonu

## 🏗️ Mimari

```
ECommercePayment/
├── Controllers/          # API Controller'lar (HTTP katmanı)
├── Application/          # İş mantığı ve DTO'lar
│   ├── DTOs/            # Veri Transfer Objeleri
│   ├── Enums/           # Uygulama enum'ları
│   ├── Interfaces/      # Servis arayüzleri
│   └── Services/        # İş mantığı implementasyonu
├── Domain/              # Temel domain varlıkları ve exception'lar
│   ├── Entities/        # Domain varlıkları
│   └── Exceptions/      # Özel exception'lar
├── Infrastructure/      # Harici endişeler
│   ├── BalanceManagement/  # Harici servis entegrasyonu
│   ├── Persistence/        # Veri kalıcılığı
│   └── Resilience/         # Dayanıklılık politikaları
└── Middleware/          # Kesişen endişeler
```

## ✨ Özellikler

### Temel Özellikler
- **Clean Architecture** implementasyonu SOLID prensipleri ile
- **Swagger/OpenAPI** dokümantasyonu detaylı örneklerle
- **Global Exception Handling** yapılandırılmış hata yanıtları ile
- **Dependency Injection** uygulama genelinde
- **Production-ready loglama** istek izleme ile

### Dayanıklılık ve Hata Toleransı
- **Retry Policy** exponential backoff ile (2s, 4s, 8s)
- **Circuit Breaker** deseni (5 hata → 30s mola)
- **Timeout Handling** (istek başına 10s, genel 30s)
- **Jitter** thundering herd problemlerini önlemek için
- **Health Monitoring** ve servis erişilebilirlik takibi

### Veri Yönetimi
- **SQL Server veritabanı** Entity Framework Core ile
- **Repository pattern** veri erişim soyutlaması için
- **Type-safe DTO'lar** validation attribute'ları ile
- **Enum-tabanlı durum yönetimi**
- **Yapılandırılmış hata yanıtları** trace ID'leri ile

## 🚀 API Endpoint'leri

### Ürünler

#### GET /api/products
Balance Management servisinden mevcut tüm ürünleri getirir.

**Yanıt Örneği:**
```json
[
  {
    "id": "prod-001",
    "name": "Premium Akıllı Telefon",
    "description": "Gelişmiş özelliklerle en son model",
    "price": 19.99,
    "currency": "USD",
    "category": "Elektronik",
    "stock": 42
  }
]
```

**Durum Kodları:**
- `200 OK` - Ürünler başarıyla getirildi
- `503 Service Unavailable` - Balance Management servisi kullanılamıyor

### Siparişler

#### POST /api/orders/create
Yeni sipariş oluşturur ve Balance Management üzerinden fonları rezerve eder.

**İstek Gövdesi:**
```json
{
  "productId": "prod-001",
  "quantity": 2
}
```

**Yanıt Örneği:**
```json
{
  "id": 1234,
  "productId": "prod-001", 
  "quantity": 2,
  "totalAmount": 39.98,
  "status": "Pending"
}
```

**Durum Kodları:**
- `200 OK` - Sipariş başarıyla oluşturuldu
- `400 Bad Request` - Geçersiz istek verisi
- `402 Payment Required` - Yetersiz bakiye
- `404 Not Found` - Ürün bulunamadı
- `503 Service Unavailable` - Balance Management servisi kullanılamıyor

#### POST /api/orders/{id}/complete
Mevcut siparişi tamamlar ve ödemeyi sonuçlandırır.

**Yanıt Örneği:**
```json
{
  "id": 1234,
  "productId": "prod-001",
  "quantity": 2, 
  "totalAmount": 39.98,
  "status": "Completed"
}
```

**Durum Kodları:**
- `200 OK` - Sipariş başarıyla tamamlandı
- `400 Bad Request` - Geçersiz sipariş ID'si
- `404 Not Found` - Sipariş bulunamadı
- `503 Service Unavailable` - Balance Management servisi kullanılamıyor

## 🛡️ Dayanıklılık Desenleri

API'miz harici servis hatalarını zarif bir şekilde ele almak için production-ready dayanıklılık desenleri uygular.

### Exponential Backoff ile Retry Policy

**Konfigürasyon:**
- **Retry Sayısı:** 3 deneme
- **Backoff Stratejisi:** Exponential (2s, 4s, 8s)
- **Jitter:** 0-1000ms rastgele gecikme
- **Tetikleyiciler:** Ağ hataları, 5XX yanıtları, timeout'lar

**Örnek Akış:**
```
İstek → Ağ Hatası → 2s Bekle → Tekrar Dene
Hala Hata → 4s Bekle → Tekrar Dene  
Hala Hata → 8s Bekle → Son Deneme
Başarılı → Yanıt Döndür
```

### Circuit Breaker Deseni

**Konfigürasyon:**
- **Hata Eşiği:** 5 ardışık hata
- **Mola Süresi:** 30 saniye
- **Kurtarma:** Başarıda otomatik sıfırlama

**Durumlar:**
- **Closed:** Normal çalışma
- **Open:** Hızla başarısız, istek gönderilmiyor
- **Half-Open:** Sınırlı isteklerle test

### Timeout Yönetimi

**Konfigürasyon:**
- **İstek Başına Timeout:** 10 saniye
- **Genel Timeout:** 30 saniye
- **Davranış:** Timeout retry mekanizmasını tetikler

### Faydalar

✅ **Ağ Hataları:** Akıllı backoff ile otomatik retry  
✅ **Servis Downtime:** Circuit breaker basamaklı hataları önler  
✅ **Yavaş Yanıtlar:** Timeout handling yanıt verme hızını korur  
✅ **Yüksek Yük:** Jitter thundering herd problemlerini önler  

## 🚀 Başlangıç

### Önkoşullar
- .NET 8 SDK
- Visual Studio 2022 veya VS Code
- SQL Server LocalDB (Visual Studio ile birlikte gelir)
- İnternet bağlantısı (Balance Management servisi için)

### Uygulamayı Çalıştırma

1. **Repository'yi klonlayın:**
```bash
git clone <repository-url>
cd ECommercePayment
```

2. **Bağımlılıkları restore edin:**
```bash
dotnet restore
```

3. **Projeyi build edin:**
```bash
dotnet build
```

4. **Veritabanını güncelleyin:**
```bash
dotnet ef database update
```

5. **Uygulamayı çalıştırın:**
```bash
dotnet run
```

6. **Swagger UI'ya erişin:**
- HTTP: `http://localhost:5215`
- HTTPS: `https://localhost:7100`

### Visual Studio

1. `ECommercePayment.sln` dosyasını açın
2. **"https"** profilini seçin
3. Debug başlatmak için **F5** tuşuna basın
4. Browser `https://localhost:7100` adresinde açılacak

## ⚙️ Konfigürasyon

### Harici Servisler

Uygulama Balance Management servisi ile entegre olur. Konfigürasyon kod içinde yapılır:

- **Base URL**: `https://balance-management-pi44.onrender.com`
- **Timeout**: Genel 30 saniye, istek başına 10 saniye
- **Retry Sayısı**: Exponential backoff ile 3 deneme
- **Circuit Breaker**: 5 ardışık hata 30 saniyelik molanı tetikler

Veritabanı konfigürasyonu `appsettings.json` dosyasında:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=ECommercePaymentDb;Trusted_Connection=true;MultipleActiveResultSets=true;TrustServerCertificate=true"
  }
}
```

### Dayanıklılık Politikaları

Politikalar `Infrastructure/Resilience/ResiliencePolicies.cs` dosyasında konfigüre edilir:

- **Retry:** Jitter ile exponential backoff
- **Circuit Breaker:** Fail-fast koruması  
- **Timeout:** İstek seviyesi timeout'lar
- **Combined Policy:** Tüm desenler birlikte çalışır

## 🔥 Hata Yönetimi

### Global Exception Middleware

Tüm exception'lar `ExceptionHandlingMiddleware` tarafından işlenir ve şunları sağlar:

- **Yapılandırılmış JSON yanıtları**
- **Uygun HTTP durum kodları**  
- **Debug için istek trace ID'leri**
- **Detaylı loglama**

### Hata Yanıt Formatı

```json
{
  "status": 503,
  "error": "Ürünler getirilirken hata: Bağlantı timeout",
  "traceId": "0HNE8E0FPA7TM:0000000B", 
  "timestamp": "2025-07-21T16:04:33.5446851Z"
}
```

### Özel Exception'lar

- **`ExternalServiceException`** - Balance Management hataları
- **`OrderNotFoundException`** - Sipariş bulunamadı senaryoları  
- **`InsufficientBalanceException`** - Ödeme hataları
- **`ValidationException`** - Girdi doğrulama hataları

## 🧪 Test

### Test Yapısı

```
ECommercePayment.Tests/
├── Controllers/          # Controller entegrasyon testleri
├── Services/            # İş mantığı unit testleri  
└── Infrastructure/      # Harici servis testleri
```

### Testleri Çalıştırma

```bash
# Tüm testleri çalıştır
dotnet test

# Coverage ile çalıştır
dotnet test --collect:"XPlat Code Coverage"

# Coverage raporu oluştur
reportgenerator -reports:**/coverage.cobertura.xml -targetdir:coverage
```

### Test Kategorileri

- **Unit Tests:** Mock'lanmış bağımlılıklarla iş mantığı
- **Integration Tests:** End-to-end API testleri
- **Resilience Tests:** Ağ hatası simülasyonu

## 📁 Proje Yapısı

### Temel Katmanlar

**Controllers** (`Controllers/`)
- `ProductsController` - Ürün kataloğu endpoint'leri
- `OrdersController` - Sipariş yönetimi endpoint'leri

**Application** (`Application/`)
- `DTOs/` - Validation ile veri transfer objeleri
- `Services/` - İş mantığı implementasyonu
- `Interfaces/` - Servis sözleşmeleri
- `Enums/` - Uygulama enum'ları

**Domain** (`Domain/`)
- `Entities/` - Temel iş varlıkları (Product, Order)
- `Exceptions/` - Domain'e özel exception'lar

**Infrastructure** (`Infrastructure/`)
- `BalanceManagement/` - Harici servis entegrasyonu
- `Persistence/` - Entity Framework Core ve Repository implementasyonları
- `Resilience/` - Hata tolerans politikaları (Polly)

## 🛠️ Teknoloji Stack

### Temel Teknolojiler
- **.NET 8** - Runtime ve framework
- **ASP.NET Core** - Web API framework
- **C#** - Programlama dili

### Kütüphaneler ve Paketler
- **Swashbuckle.AspNetCore** - OpenAPI/Swagger dokümantasyonu
- **Microsoft.Extensions.Http.Polly** - Dayanıklılık desenleri
- **Microsoft.EntityFrameworkCore.SqlServer** - SQL Server veri erişimi
- **Microsoft.EntityFrameworkCore.Tools** - EF Core CLI araçları
- **System.Text.Json** - JSON serialization

### Mimari Desenler
- **Clean Architecture** - Endişelerin ayrılması
- **Dependency Injection** - Kontrol inversiyonu
- **Repository Pattern** - Veri erişim soyutlaması
- **Retry Pattern** - Hata toleransı
- **Circuit Breaker** - Servis koruması

### Geliştirme Araçları
- **Visual Studio 2022** - IDE
- **Swagger UI** - API testleri
- **HTTP Client** - Harici servis entegrasyonu

## 📊 Monitoring ve Gözlemlenebilirlik

### Loglama
- **Yapılandırılmış loglama** istek korelasyonu ile
- **Retry deneme takibi** 
- **Circuit breaker durum değişiklikleri**
- **Performans metrikleri**

### Veritabanı ve Kalıcılık
- **Entity Framework Core** SQL Server ile
- **Migration desteği** veritabanı şema değişiklikleri için
- **Repository pattern** implementasyonu
- **Connection string** konfigürasyonu appsettings'de

## 🔒 Güvenlik Hususları

- **Girdi doğrulama** tüm endpoint'lerde
- **Exception sanitization** yanıtlarda
- **Timeout koruması** slowloris saldırılarına karşı
- **Rate limiting** circuit breaker üzerinden

## 🚀 Production Hazırlığı

Bu API production deployment için tasarlanmıştır:

✅ **Harici bağımlılıklar için hata toleransı**  
✅ **Servis kesintilerinde zarif bozulma**  
✅ **Yerleşik monitoring ve gözlemlenebilirlik**  
✅ **Temiz hata yönetimi ve kullanıcı deneyimi**  
✅ **Gelecekteki büyümeyi destekleyen ölçeklenebilir mimari**  

## 🇹🇷 Türkçe Dokümantasyon

Bu dokümantasyon Türkçe olarak hazırlanmıştır. İngilizce versiyon için [README.md](README.md) dosyasına göz atın. 