﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Aprojectbackend.Models;

public partial class THobbyPrefer
{
    public int FHobbyPreferId { get; set; }

    public int? FUserPreferId { get; set; }

    public int? FHobbyId { get; set; }

    public virtual THobby FHobby { get; set; }

    public virtual TUserPrefer FUserPrefer { get; set; }
}