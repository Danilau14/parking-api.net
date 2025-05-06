// System
global using System.ComponentModel.DataAnnotations;
global using System.ComponentModel.DataAnnotations.Schema;
global using System.Diagnostics.CodeAnalysis;
global using System.IdentityModel.Tokens.Jwt;
global using System.Net;
global using System.Net.Mail;
global using System.Security.Claims;
global using System.Text;
global using System.Text.Json;

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

global using MediatR;

//AWS
global using Amazon;
global using Amazon.DynamoDBv2;
global using Amazon.DynamoDBv2.DataModel;
global using Amazon.DynamoDBv2.DocumentModel;



// AutoMapper
global using AutoMapper;
global using Profile = AutoMapper.Profile;

// RabbitMQ
global using RabbitMQ.Client;
global using RabbitMQ.Client.Events;

// ParkingApi - Data, Models, DTOs, Mappings, Repositories, Services, Interfaces
global using ParkingApi.Core;

global using ParkingApi.Application.Common.Dtos;
global using ParkingApi.Application.Common.Exceptions;
global using ParkingApi.Application.Common.Converters;
global using ParkingApi.Application.Features.Auth.Commands;
global using ParkingApi.Application.Features.Auth.Dtos;
global using ParkingApi.Application.Features.Emails.Events;
global using ParkingApi.Application.Features.Emails.Dtos;
global using ParkingApi.Application.Features.ParkingLots.Commands;
global using ParkingApi.Application.Features.ParkingLots.Queries;
global using ParkingApi.Application.Features.ParkingLots.Dtos;
global using ParkingApi.Application.Features.Users.Commands;
global using ParkingApi.Application.Features.Users.Dtos;
global using ParkingApi.Application.Features.ParkingHistories.Dtos;
global using ParkingApi.Application.Features.ParkingHistories.Commands;
global using ParkingApi.Application.Features.ParkingHistories.Queries;
global using ParkingApi.Application.Mappings.ParkingHistories;
global using ParkingApi.Application.Mappings.ParkingsLot;
global using ParkingApi.Application.Mappings.Users;
global using ParkingApi.Application.Messaging;

global using ParkingApi.Application.Services;
global using ParkingApi.Application.Services.RabbitMQ;
global using ParkingApi.Application.Services.RabbitMQ.Publisher;


global using ParkingApi.Core.Enums;
global using ParkingApi.Core.Interfaces;
global using ParkingApi.Core.Global;
global using ParkingApi.Core.Models;

global using ParkingApi.Infrastructure;
global using ParkingApi.Infrastructure.Data;
global using ParkingApi.Infrastructure.Extensions;
global using ParkingApi.Infrastructure.Repositories;
global using ParkingApi.Infrastructure.RepositoriesDynamo;

global using AmazonProfile = Amazon.Profile;
