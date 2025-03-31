# Authorization

This API uses JWT tokens.

To authorize:
1. Send a POST request to `/api/auth/login` with your credentials.
2. Copy the returned JWT token.
3. Use the token in the Authorization header for all protected endpoints:

```
Authorization: Bearer SuperSecretKey123456789!@#$%^&*()
```
