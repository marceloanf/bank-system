# Run the solution and open swagger (http://localhost:5202/swagger/index.html)

##Create an user:
Endpoint: users POST
Payload:
{
  "name": "Marcelo",
  "accounts": []
}

##Create account
Endpoint: accounts POST
Payload:
{
  "balance": 5000,
  "userId": 1
}


Then run any operation using the other endpoints
