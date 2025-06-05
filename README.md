# Technical Specification Document

## General Information
**Project Name:** DigitalShop_Sample  
**Repository:** [GitHub - DigitalShop_Sample](https://github.com/o-otruta/DigitalShop_Sample)  
**Goal:** The purpose of this project is to develop an MVP (Minimum Viable Product) for an online store that sells digital gaming assets, including accounts, in-game currency, and resources.  

## Functional Requirements

### Main Page
- Display a list of products with basic info and optional images.
- Pagination of product list.
- Filtering products by category or other criteria.
- Sorting products by price or other fields.

### Product Page
- Detailed information: name, description, price, category.
- “Buy” button.

### Purchase Modal
- Product summary and quantity input (if needed).
- Ability to cancel purchase.
- Option to select payment method.
- “Place Order” button.
- Confirmation message (success/failure).

### Purchase History
- View past orders sorted from newest to oldest.

### Authentication
- Registration (without email confirmation).
- Login/logout.
- Passwords stored with hashing.

### Admin Panel
- View product list (same as user-facing view).
- Create/edit/delete products.
- View order list.

## Non-Functional Requirements
- Built using .NET 6.
- Blazor WebAssembly frontend.
- PostgreSQL (or SQL Server) for data storage.
- Responsive layout (desktop only).
- Bootstrap or any CSS framework.
- English-language interface.

## Database Structure
- **Users:** Includes hashed passwords, roles (admin/client).
- **Products:** Name, description, price, category, image.
- **Orders:** Linked to users and products.

Test data must include:
- At least 10 products.
- At least 2 users (admin and client).

## Technologies Used
- **Backend:** .NET 6+, Entity Framework Core.
- **Frontend:** Blazor WebAssembly, Bootstrap.
- **Database:** PostgreSQL.
- **Payment Integration:** LiqPay (test mode).

## Optional Improvements (Implemented)
- Search by product name and category.
- Real-time payment integration using LiqPay test mode.

## Acceptance and Testing
- Manual testing of all UI features.
- Functional testing of API and payment flow.
- Admin functions and DB validation.

## Maintenance
- Code maintained via GitHub.
- Issues managed using GitHub Issues tracker.

## Deployment and Access
- Project available on GitHub.
- Documentation hosted on GitHub Pages.

## Links
- Repository: https://github.com/o-otruta/DigitalShop_Sample
- API Documentation: https://o-otruta.github.io/DigitalShop_Sample/zudoku/docs/about

