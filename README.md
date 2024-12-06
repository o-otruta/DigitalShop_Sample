Development of a minimum viable product. An online store for the sale of game accounts, game currency, and resources using Blazor, .NET, and PostgreSQL technologies.

```sql
-- Creating tables manualy
CREATE TABLE users (
    id SERIAL PRIMARY KEY,
    username VARCHAR(255) NOT NULL,
    passwordhash VARCHAR(255) NOT NULL,
    role VARCHAR(50) DEFAULT 'User'
);

CREATE TABLE products (
    id SERIAL PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    description TEXT,
    price DECIMAL(10, 2) NOT NULL,
    quantity INT DEFAULT 0 NOT NULL,
    category VARCHAR(255),
    image_url VARCHAR(255),
    game VARCHAR(255) NOT NULL,
    game_key VARCHAR(255) NOT NULL,
    created_by VARCHAR(255),
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE orders (
    id SERIAL PRIMARY KEY,
    product_id INT NOT NULL,
    product_name VARCHAR(255) NOT NULL,
    product_game VARCHAR(255) NOT NULL,
    product_category VARCHAR(255) NOT NULL,
    buyer_name VARCHAR(255) NOT NULL,
    seller_name VARCHAR(255) NOT NULL,
    price DECIMAL(10, 2) NOT NULL,
    quantity INT DEFAULT 1 NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (product_id) REFERENCES products (id)
);
```
