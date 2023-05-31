//.NET
global using System;
global using Microsoft.Extensions.Options;


//NUGET/ mongo
global using MongoDB.Bson;
global using MongoDB.Bson.Serialization.Attributes;
global using MongoDB.Driver;

//validation
global using FluentValidation;
global using FluentValidation.AspNetCore;

//JWT Tokens
global using Microsoft.IdentityModel.Tokens;
global using System.IdentityModel.Tokens.Jwt;
global using System.Security.Claims;
global using Microsoft.AspNetCore.Authorization;

//encoding
global using System.Text;



//Customer
global using Shops.Models;
global using Shops.Configuration;
global using Shops.Context;
global using Shops.Repositories;
global using Shops.ShopServices;
global using Shops.GraphQl.Querries;
global using Shops.GraphQl.Mutation;
global using Shops.Validator;


