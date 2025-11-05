# Customer CRUD API - Hý?ng D?n S? D?ng

## Endpoints

### 1. L?y t?t c? khách hàng
```http
GET /api/customer
```
**Response:**
```json
{
  "status": 200,
  "message": "Customers retrieved successfully",
  "data": [
    {
      "customerId": 1,
      "name": "Nguy?n Vãn A",
      "phone": "0123456789",
      "email": "a@example.com",
      "address": "123 Ðý?ng ABC, Hà N?i",
      "createdAt": "2024-01-15T10:30:00Z",
      "orderCount": 5
    }
  ]
}
```

### 2. L?y khách hàng theo ID
```http
GET /api/customer/{id}
```
**Example:** `GET /api/customer/1`

**Response:**
```json
{
  "status": 200,
  "message": "Customer retrieved successfully",
  "data": {
    "customerId": 1,
    "name": "Nguy?n Vãn A",
    "phone": "0123456789",
    "email": "a@example.com",
    "address": "123 Ðý?ng ABC, Hà N?i",
    "createdAt": "2024-01-15T10:30:00Z",
    "orderCount": 5
  }
}
```

### 3. T?o khách hàng m?i
```http
POST /api/customer
Content-Type: application/json
```
**Request Body:**
```json
{
  "name": "Nguy?n Vãn A",
  "phone": "0123456789",
  "email": "a@example.com",
  "address": "123 Ðý?ng ABC, Hà N?i"
}
```
**Note:** Ch? `name` là b?t bu?c, các field khác là tùy ch?n.

**Response:**
```json
{
  "status": 200,
  "message": "Customer created successfully",
  "data": {
    "customerId": 1,
    "name": "Nguy?n Vãn A",
    "phone": "0123456789",
    "email": "a@example.com",
    "address": "123 Ðý?ng ABC, Hà N?i",
    "createdAt": "2024-01-15T10:30:00Z",
    "orderCount": 0
  }
}
```

### 4. C?p nh?t khách hàng
```http
PUT /api/customer/{id}
Content-Type: application/json
```
**Example:** `PUT /api/customer/1`

**Request Body:**
```json
{
  "name": "Nguy?n Vãn B",
  "phone": "0987654321",
  "email": "b@example.com",
  "address": "456 Ðý?ng XYZ, Hà N?i"
}
```

**Response:**
```json
{
  "status": 200,
  "message": "Customer updated successfully",
  "data": {
    "customerId": 1,
    "name": "Nguy?n Vãn B",
    "phone": "0987654321",
    "email": "b@example.com",
    "address": "456 Ðý?ng XYZ, Hà N?i",
    "createdAt": "2024-01-15T10:30:00Z",
    "orderCount": 5
  }
}
```

### 5. Xóa khách hàng
```http
DELETE /api/customer/{id}
```
**Example:** `DELETE /api/customer/1`

**Response:**
```json
{
  "status": 200,
  "message": "Customer deleted successfully",
  "data": null
}
```

### 6. L?y khách hàng theo Email
```http
GET /api/customer/email/{email}
```
**Example:** `GET /api/customer/email/a@example.com`

**Response:**
```json
{
  "status": 200,
  "message": "Customer retrieved successfully",
  "data": {
    "customerId": 1,
    "name": "Nguy?n Vãn A",
    "phone": "0123456789",
    "email": "a@example.com",
    "address": "123 Ðý?ng ABC, Hà N?i",
    "createdAt": "2024-01-15T10:30:00Z",
    "orderCount": 5
  }
}
```

### 7. L?y khách hàng theo Phone
```http
GET /api/customer/phone/{phone}
```
**Example:** `GET /api/customer/phone/0123456789`

**Response:**
```json
{
  "status": 200,
  "message": "Customer retrieved successfully",
  "data": {
    "customerId": 1,
    "name": "Nguy?n Vãn A",
    "phone": "0123456789",
    "email": "a@example.com",
    "address": "123 Ðý?ng ABC, Hà N?i",
    "createdAt": "2024-01-15T10:30:00Z",
    "orderCount": 5
  }
}
```

### 8. T?m ki?m khách hàng theo Tên
```http
GET /api/customer/search?name={name}
```
**Example:** `GET /api/customer/search?name=Nguy?n`

**Response:**
```json
{
  "status": 200,
  "message": "Customers retrieved successfully",
  "data": [
    {
      "customerId": 1,
      "name": "Nguy?n Vãn A",
      "phone": "0123456789",
      "email": "a@example.com",
      "address": "123 Ðý?ng ABC, Hà N?i",
      "createdAt": "2024-01-15T10:30:00Z",
      "orderCount": 5
    },
    {
      "customerId": 2,
      "name": "Nguy?n Vãn B",
      "phone": "0987654321",
      "email": "b@example.com",
      "address": "456 Ðý?ng XYZ, Hà N?i",
      "createdAt": "2024-01-16T14:20:00Z",
      "orderCount": 3
    }
  ]
}
```

## Validation Rules

### T?o khách hàng (POST)
- `name` (Required): T?i ða 100 k? t?
- `phone` (Optional): T?i ða 20 k? t?
- `email` (Optional): Ph?i là ð?nh d?ng email h?p l?, t?i ða 100 k? t?
- `address` (Optional): Không gi?i h?n ð? dài

### C?p nh?t khách hàng (PUT)
- Gi?ng nhý POST
- **Lýu ?:** Email và Phone ph?i là duy nh?t (không ðý?c trùng l?p v?i khách hàng khác)

## Error Responses

### 400 - Bad Request
```json
{
  "status": 400,
  "message": "A customer with this email already exists",
  "data": null
}
```

### 404 - Not Found
```json
{
  "status": 404,
  "message": "Customer with ID 999 not found",
  "data": null
}
```

### 500 - Internal Server Error
```json
{
  "status": 500,
  "message": "Error retrieving customers: ...",
  "data": null
}
```

## Các tính nãng chính

1. ? **Get All** - L?y t?t c? khách hàng
2. ? **Get By ID** - L?y khách hàng theo ID
3. ? **Create** - T?o khách hàng m?i (ch? c?n 1 request)
4. ? **Update** - C?p nh?t khách hàng (id trong URL, d? li?u trong body)
5. ? **Delete** - Xóa khách hàng
6. ? **Get By Email** - T?m ki?m theo email
7. ? **Get By Phone** - T?m ki?m theo s? ði?n tho?i
8. ? **Search By Name** - T?m ki?m theo tên (LIKE query)
9. ? **Validation** - Ki?m tra email/phone không trùng l?p
10. ? **Order Count** - Hi?n th? s? lý?ng ðõn hàng c?a khách hàng

## C?u trúc project

```
StoreManagement/
??? Controllers/
?   ??? CustomerController.cs
??? Services/
?   ??? ICustomerService.cs
?   ??? Impl/
?       ??? CustomerService.cs
??? Repository/
?   ??? ICustomerRepository.cs
?   ??? Impl/
?       ??? CustomerRepository.cs
??? DTOs/
?   ??? Request/
?   ?   ??? CustomerCreateRequest.cs
?   ?   ??? CustomerUpdateRequest.cs
?   ??? Response/
?       ??? CustomerResponse.cs
??? Mapper/
    ??? CustomerMapper.cs
```

## Mô t? thêm

- **Lýu ?:** T?t c? timestamps (CreatedAt) ðý?c lýu dý?i d?ng UTC
- **Validate:** Email ph?i ðúng ð?nh d?ng email, Phone và Email ph?i là duy nh?t
- **Response:** S? d?ng class `Response` chung cho toàn project v?i ð?nh d?ng `{ Status, Message, Data }`
