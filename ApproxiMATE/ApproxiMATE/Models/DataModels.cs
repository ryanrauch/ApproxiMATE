﻿using ApproxiMATE.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApproxiMATE.Models
{
    public class ZoneState
    {
        public int StateId { get; set; }
        public String Description { get; set; }         //"Texas"
        public String ShortDescription { get; set; }    //"TX"
    }

    public class ZoneCity
    {
        public int CityId { get; set; }
        public String Description { get; set; }
        public virtual ZoneState State { get; set; }
    }

    public class ApplicationOption
    {
        public int OptionsId { get; set; }
        public DateTime OptionsDate { get; set; }
        public string EndUserLicenseAgreementSource { get; set; }
        public string TermsConditionsSource { get; set; }
        public string PrivacyPolicySource { get; set; }
        public TimeSpan DataTimeWindow { get; set; }
        public int Version { get; set; }
        public int VersionMajor { get; set; }
        public int VersionMinor { get; set; }

    }

    public class ApplicationUser
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public DateTime dateOfBirth { get; set; }
        public int gender { get; set; }
        public int accountType { get; set; }
        public DateTime? termsAndConditionsDate { get; set; }
        public double? currentLatitude { get; set; }
        public double? currentLongitude { get; set; }
        public DateTime? currentTimeStamp { get; set; }
        public Guid id { get; set; }
        public string userName { get; set; }
        public string normalizedUserName { get; set; }
        public string email { get; set; }
        public string normalizedEmail { get; set; }
        public bool emailConfirmed { get; set; }
        public string passwordHash { get; set; }
        public string securityStamp { get; set; }
        public string concurrencyStamp { get; set; }
        public object phoneNumber { get; set; }
        public bool phoneNumberConfirmed { get; set; }
        public bool twoFactorEnabled { get; set; }
        public object lockoutEnd { get; set; }
        public bool lockoutEnabled { get; set; }
        public int accessFailedCount { get; set; }
    }
    public class CurrentLocation
    {
        public string UserId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    public class FriendRequestOld
    {
        public Guid InitiatorId { get; set; }
        public Guid TargetId { get; set; }
        public DateTime TimeStamp { get; set; }
        public FriendRequestType? Type { get; set; }
    }

    public class UserPhoneResult
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class UserPhoneNumbers
    {
        public Guid UserId { get; set; }
        public List<string> Numbers { get; set; }
    }

    public class FriendLocationBoxRequest
    {
        public Guid UserId { get; set; }
        public string BoundingBox { get; set; }
    }

    public class FriendLocationBox
    {
        public string BoundingBox { get; set; }
        public List<FriendLocationResult> Friends { get; set; }
    }

    public class FriendLocationResult
    {
        public Guid UserId { get; set; }
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
        public DateTime Timestamp { get; set; }
    }

    public class ZoneRegionPolygon
    {
        public int RegionId { get; set; }
        public int Order { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    public class ZoneRegion
    {
        public int RegionId { get; set; }
        public String Description { get; set; }
        public int Type { get; set; }               //enum
        public virtual ZoneCity City { get; set; }  //???
        public double BoundLatitudeMin { get; set; }
        public double BoundLatitudeMax { get; set; }
        public double BoundLongitudeMin { get; set; }
        public double BoundLongitudeMax { get; set; }
        public String ARGBFill { get; set; }
        public String ARGBStroke { get; set; }
        public float StrokeWidth { get; set; }
    }
}
