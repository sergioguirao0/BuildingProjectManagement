﻿using BuildingProjectManagementAPI.Model.Dao;
using BuildingProjectManagementAPI.Model.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;
using BuildingProjectManagementAPI.Data;
using AutoMapper;
using BuildingProjectManagementAPI.Model.Dto;
using BuildingProjectManagementAPI.Model.Entities;

namespace BuildingProjectManagementAPI.Services
{
    public class UserService : IUserRepository
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly IConfiguration configuration;
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public UserService(UserManager<IdentityUser> userManager, IConfiguration configuration, ApplicationDbContext context, IMapper mapper)
        {
            this.userManager = userManager;
            this.configuration = configuration;
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<IdentityResult> RegisterUser(UserRegistrationEntity userRegistrationEntity)
        {
            var user = new IdentityUser
            {
                UserName = userRegistrationEntity.userCredentialsDTO!.Email,
                Email = userRegistrationEntity.userCredentialsDTO!.Email
            };

            var result = await userManager.CreateAsync(user, userRegistrationEntity.userCredentialsDTO.Password);

            if (!result.Succeeded)
            {
                return result;
            }

            var userEntity = mapper.Map<UserEntity>(userRegistrationEntity.userRegisterDTO);
            userEntity.AspNetUserId = user.Id;

            context.Usuarios.Add(userEntity);
            await context.SaveChangesAsync();

            return result;
        }

        public async Task<AuthenticationResponseDTO> BuildToken(UserCredentialsDTO userCredentialsDTO)
        {
            var claims = new List<Claim>
            {
                new Claim("email", userCredentialsDTO.Email)
            };

            var user = await userManager.FindByEmailAsync(userCredentialsDTO.Email);
            var claimsDB = await userManager.GetClaimsAsync(user!);

            claims.AddRange(claimsDB);

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtKey"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var expirationTime = DateTime.UtcNow.AddDays(1);

            var securityToken = new JwtSecurityToken(issuer: null, audience: null, claims: claims,
                expires: expirationTime, signingCredentials: credentials);

            var token = new JwtSecurityTokenHandler().WriteToken(securityToken);

            return new AuthenticationResponseDTO
            {
                Token = token,
                ExpirationTime = expirationTime
            };
        }
    }
}
