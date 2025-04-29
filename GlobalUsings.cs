// System
global using System.ComponentModel.DataAnnotations;
global using System.ComponentModel.DataAnnotations.Schema;
global using System.Diagnostics.CodeAnalysis;
global using System.IdentityModel.Tokens.Jwt;
global using System.Net;
global using System.Net.Mail;
global using System.Security.Claims;
global using System.Text;

// Microsoft
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Identity;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Mvc.Filters;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.Options;
global using Microsoft.IdentityModel.Tokens;
global using Microsoft.OpenApi.Models;


// AutoMapper
global using AutoMapper;

// RabbitMQ
global using RabbitMQ.Client;

// ParkingApi - Data, Models, DTOs, Mappings, Repositories, Services, Interfaces
global using ParkingApi.Core;
global using ParkingApi.Data;
global using ParkingApi.Dto.Email;
global using ParkingApi.Dto.Error;
global using ParkingApi.Dto.Pagination;
global using ParkingApi.Dto.ParkingHistory;
global using ParkingApi.Dto.ParkingsLot;
global using ParkingApi.Dto.Users;
global using ParkingApi.Exceptions;
global using ParkingApi.Extensions;
global using ParkingApi.Interfaces;
global using ParkingApi.Mappings.ParkingHistories;
global using ParkingApi.Mappings.ParkingsLot;
global using ParkingApi.Mappings.Users;
global using ParkingApi.Models;
global using ParkingApi.Models.Enums;
global using ParkingApi.Repositories;
global using ParkingApi.Services.cs;
