﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Aprojectbackend.Models;

public partial class AprojectContext : DbContext
{
    public AprojectContext(DbContextOptions<AprojectContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TActCategory> TActCategories { get; set; }

    public virtual DbSet<TActDetail> TActDetails { get; set; }

    public virtual DbSet<TActEventReg> TActEventRegs { get; set; }

    public virtual DbSet<TActImgDatum> TActImgData { get; set; }

    public virtual DbSet<TActInformation> TActInformations { get; set; }

    public virtual DbSet<TAdvertisement> TAdvertisements { get; set; }

    public virtual DbSet<TCart> TCarts { get; set; }

    public virtual DbSet<TDonation> TDonations { get; set; }

    public virtual DbSet<TForum> TForums { get; set; }

    public virtual DbSet<THobby> THobbies { get; set; }

    public virtual DbSet<THobbyPrefer> THobbyPrefers { get; set; }

    public virtual DbSet<TKeyword> TKeywords { get; set; }

    public virtual DbSet<TLight> TLights { get; set; }

    public virtual DbSet<TManager> TManagers { get; set; }

    public virtual DbSet<TMatch> TMatches { get; set; }

    public virtual DbSet<TOrder> TOrders { get; set; }

    public virtual DbSet<TOrderDetail> TOrderDetails { get; set; }

    public virtual DbSet<TPermission> TPermissions { get; set; }

    public virtual DbSet<TPost> TPosts { get; set; }

    public virtual DbSet<TPostReply> TPostReplies { get; set; }

    public virtual DbSet<TProdCategory> TProdCategories { get; set; }

    public virtual DbSet<TProduct> TProducts { get; set; }

    public virtual DbSet<TQuestionAnswer> TQuestionAnswers { get; set; }

    public virtual DbSet<TQuestionType> TQuestionTypes { get; set; }

    public virtual DbSet<TRegDetail> TRegDetails { get; set; }

    public virtual DbSet<TServiceDetail> TServiceDetails { get; set; }

    public virtual DbSet<TStatus> TStatuses { get; set; }

    public virtual DbSet<TStockDetail> TStockDetails { get; set; }

    public virtual DbSet<TTrait> TTraits { get; set; }

    public virtual DbSet<TTraitsPrefer> TTraitsPrefers { get; set; }

    public virtual DbSet<TUpcomingAct> TUpcomingActs { get; set; }

    public virtual DbSet<TUser> TUsers { get; set; }

    public virtual DbSet<TUserHobby> TUserHobbies { get; set; }

    public virtual DbSet<TUserLike> TUserLikes { get; set; }

    public virtual DbSet<TUserPrefer> TUserPrefers { get; set; }

    public virtual DbSet<TUserState> TUserStates { get; set; }

    public virtual DbSet<TUserTrait> TUserTraits { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TActCategory>(entity =>
        {
            entity.HasKey(e => e.FActCategoryId);

            entity.ToTable("tActCategory");

            entity.Property(e => e.FActCategoryId).HasColumnName("fActCategoryId");
            entity.Property(e => e.FActCategory)
                .HasMaxLength(20)
                .HasColumnName("fActCategory");
        });

        modelBuilder.Entity<TActDetail>(entity =>
        {
            entity.HasKey(e => e.FActDetailId);

            entity.ToTable("tActDetail");

            entity.Property(e => e.FActDetailId).HasColumnName("fActDetailId");
            entity.Property(e => e.FActId).HasColumnName("fActId");
            entity.Property(e => e.FDisplayId)
                .HasMaxLength(30)
                .HasColumnName("fDisplayId");
            entity.Property(e => e.FEndDate)
                .HasMaxLength(20)
                .HasColumnName("fEndDate");
            entity.Property(e => e.FRegDeadline)
                .HasMaxLength(20)
                .HasColumnName("fRegDeadline");
            entity.Property(e => e.FRegStartDate)
                .HasMaxLength(20)
                .HasColumnName("fRegStartDate");
            entity.Property(e => e.FStartDate)
                .HasMaxLength(20)
                .HasColumnName("fStartDate");
            entity.Property(e => e.FStatusId).HasColumnName("fStatusId");

            entity.HasOne(d => d.FAct).WithMany(p => p.TActDetails)
                .HasForeignKey(d => d.FActId)
                .HasConstraintName("FK_tActDetail_tActInformation");
        });

        modelBuilder.Entity<TActEventReg>(entity =>
        {
            entity.HasKey(e => e.FRegId).HasName("PK_tEventReg");

            entity.ToTable("tActEventReg");

            entity.Property(e => e.FRegId).HasColumnName("fRegId");
            entity.Property(e => e.FActDetailId).HasColumnName("fActDetailId");
            entity.Property(e => e.FActPayment)
                .HasColumnType("money")
                .HasColumnName("fActPayment");
            entity.Property(e => e.FDispRegNum)
                .HasMaxLength(50)
                .HasColumnName("fDispRegNum");
            entity.Property(e => e.FRegDate)
                .HasMaxLength(20)
                .HasColumnName("fRegDate");
            entity.Property(e => e.FStatusId).HasColumnName("fStatusId");
            entity.Property(e => e.FUserId).HasColumnName("fUserId");

            entity.HasOne(d => d.FStatus).WithMany(p => p.TActEventRegs)
                .HasForeignKey(d => d.FStatusId)
                .HasConstraintName("FK_tEventReg_tStatus");
        });

        modelBuilder.Entity<TActImgDatum>(entity =>
        {
            entity.HasKey(e => e.FImgId).HasName("PK__tImgData__2090D768B11D7674");

            entity.ToTable("tActImgData");

            entity.Property(e => e.FImgId).HasColumnName("fImgId");
            entity.Property(e => e.FActId).HasColumnName("fActId");
            entity.Property(e => e.FActImgName)
                .HasMaxLength(50)
                .HasColumnName("fActImgName");
            entity.Property(e => e.FActImgPath).HasColumnName("fActImgPath");
            entity.Property(e => e.FImgByte).HasColumnName("fImgByte");
            entity.Property(e => e.FImgDescription)
                .HasMaxLength(50)
                .HasColumnName("fImgDescription");

            entity.HasOne(d => d.FAct).WithMany(p => p.TActImgData)
                .HasForeignKey(d => d.FActId)
                .HasConstraintName("FK_tActImgData_tActInformation");
        });

        modelBuilder.Entity<TActInformation>(entity =>
        {
            entity.HasKey(e => e.FActId);

            entity.ToTable("tActInformation");

            entity.Property(e => e.FActId).HasColumnName("fActId");
            entity.Property(e => e.FActCategoryId).HasColumnName("fActCategoryId");
            entity.Property(e => e.FActDescription).HasColumnName("fActDescription");
            entity.Property(e => e.FActDisplayId)
                .HasMaxLength(30)
                .HasColumnName("fActDisplayId");
            entity.Property(e => e.FActLocation)
                .HasMaxLength(20)
                .HasColumnName("fActLocation");
            entity.Property(e => e.FActName)
                .HasMaxLength(20)
                .HasColumnName("fActName");
            entity.Property(e => e.FActUpdateDate)
                .HasMaxLength(20)
                .HasColumnName("fActUpdateDate");
            entity.Property(e => e.FCompanyId).HasColumnName("fCompanyId");
            entity.Property(e => e.FMaxNumber).HasColumnName("fMaxNumber");
            entity.Property(e => e.FRegFee)
                .HasColumnType("money")
                .HasColumnName("fRegFee");

            entity.HasOne(d => d.FActCategory).WithMany(p => p.TActInformations)
                .HasForeignKey(d => d.FActCategoryId)
                .HasConstraintName("FK_tActInformation_tActCategory");
        });

        modelBuilder.Entity<TAdvertisement>(entity =>
        {
            entity.HasKey(e => e.FAdId).HasName("PK__tAdverti__B4695C10ACDA5F4E");

            entity.ToTable("tAdvertisement");

            entity.Property(e => e.FAdId).HasColumnName("fAdId");
            entity.Property(e => e.FAdName)
                .HasMaxLength(50)
                .HasColumnName("fAdName");
            entity.Property(e => e.FAdType)
                .HasMaxLength(50)
                .HasColumnName("fAdType");
            entity.Property(e => e.FClickCount).HasColumnName("fClickCount");
            entity.Property(e => e.FImagePath)
                .HasMaxLength(255)
                .HasColumnName("fImagePath");
            entity.Property(e => e.FLink)
                .HasMaxLength(255)
                .HasColumnName("fLink");
            entity.Property(e => e.FUserId).HasColumnName("fUserId");
            entity.Property(e => e.UserId會員id)
                .HasMaxLength(50)
                .HasColumnName("UserId 會員ID");
        });

        modelBuilder.Entity<TCart>(entity =>
        {
            entity.HasKey(e => e.FCartItemId).HasName("PK__tCart__6174C4BDDA9B90A4");

            entity.ToTable("tCart");

            entity.Property(e => e.FCartItemId).HasColumnName("fCartItemId");
            entity.Property(e => e.FItemQuantity).HasColumnName("fItemQuantity");
            entity.Property(e => e.FProductId).HasColumnName("fProductId");
            entity.Property(e => e.FUserId).HasColumnName("fUserId");
        });

        modelBuilder.Entity<TDonation>(entity =>
        {
            entity.HasKey(e => e.FDonationId).HasName("PK__tDonatio__C13E45BB2EC9001D");

            entity.ToTable("tDonation");

            entity.Property(e => e.FDonationId).HasColumnName("fDonationId");
            entity.Property(e => e.FAmount)
                .HasColumnType("money")
                .HasColumnName("fAmount");
            entity.Property(e => e.FDonationDate)
                .HasMaxLength(20)
                .HasColumnName("fDonationDate");
            entity.Property(e => e.FUserId).HasColumnName("fUserId");
        });

        modelBuilder.Entity<TForum>(entity =>
        {
            entity.HasKey(e => e.ForId).HasName("PK_Forums");

            entity.ToTable("tForums");

            entity.Property(e => e.ForDescription).IsRequired();
            entity.Property(e => e.ForTitle).IsRequired();
            entity.Property(e => e.ImageUrl).IsRequired();
        });

        modelBuilder.Entity<THobby>(entity =>
        {
            entity.HasKey(e => e.FHobbyId).HasName("PK__tHobby__57F4F4702122A10C");

            entity.ToTable("tHobby");

            entity.Property(e => e.FHobbyId).HasColumnName("fHobbyId");
            entity.Property(e => e.FHobbyName)
                .HasMaxLength(50)
                .HasColumnName("fHobbyName");
        });

        modelBuilder.Entity<THobbyPrefer>(entity =>
        {
            entity.HasKey(e => e.FHobbyPreferId).HasName("PK__tHobbyPr__247BD02BCB41C0F6");

            entity.ToTable("tHobbyPrefer");

            entity.Property(e => e.FHobbyPreferId).HasColumnName("fHobbyPreferId");
            entity.Property(e => e.FHobbyId).HasColumnName("fHobbyId");
            entity.Property(e => e.FUserPreferId).HasColumnName("fUserPreferId");

            entity.HasOne(d => d.FHobby).WithMany(p => p.THobbyPrefers)
                .HasForeignKey(d => d.FHobbyId)
                .HasConstraintName("FK_tHobbyPrefer_tHobby");

            entity.HasOne(d => d.FUserPrefer).WithMany(p => p.THobbyPrefers)
                .HasForeignKey(d => d.FUserPreferId)
                .HasConstraintName("FK_tHobbyPrefer_tUserPrefer");
        });

        modelBuilder.Entity<TKeyword>(entity =>
        {
            entity.HasKey(e => e.FKeywordDetailId).HasName("PK__tKeyword__BFF83A5F0C077358");

            entity.ToTable("tKeyword");

            entity.Property(e => e.FKeywordDetailId).HasColumnName("fKeywordDetailId");
            entity.Property(e => e.FProdCategoryId).HasColumnName("fProdCategoryId");
            entity.Property(e => e.FProdKeyword)
                .HasMaxLength(50)
                .HasColumnName("fProdKeyword");
        });

        modelBuilder.Entity<TLight>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("tLight");

            entity.Property(e => e.FLightAddress)
                .HasMaxLength(50)
                .HasColumnName("fLightAddress");
            entity.Property(e => e.FLightBirthday).HasColumnName("fLightBirthday");
            entity.Property(e => e.FLightEnd).HasColumnName("fLightEnd");
            entity.Property(e => e.FLightId).HasColumnName("fLightId");
            entity.Property(e => e.FLightStart).HasColumnName("fLightStart");
            entity.Property(e => e.FLightStatus).HasColumnName("fLightStatus");
            entity.Property(e => e.FLightUser)
                .HasMaxLength(50)
                .HasColumnName("fLightUser");
            entity.Property(e => e.FUserId).HasColumnName("fUserId");
        });

        modelBuilder.Entity<TManager>(entity =>
        {
            entity.HasKey(e => e.FManagerId).HasName("PK__tManager__A628137881930CC4");

            entity.ToTable("tManager");

            entity.Property(e => e.FManagerId).HasColumnName("fManagerId");
            entity.Property(e => e.FManagerActivityState).HasColumnName("fManagerActivityState");
            entity.Property(e => e.FManagerAddress)
                .HasMaxLength(255)
                .HasColumnName("fManagerAddress");
            entity.Property(e => e.FManagerAdvertisementState).HasColumnName("fManagerAdvertisementState");
            entity.Property(e => e.FManagerEmail)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("fManagerEmail");
            entity.Property(e => e.FManagerMatchState).HasColumnName("fManagerMatchState");
            entity.Property(e => e.FManagerName)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("fManagerName");
            entity.Property(e => e.FManagerOrderState).HasColumnName("fManagerOrderState");
            entity.Property(e => e.FManagerPassword)
                .IsRequired()
                .IsUnicode(false)
                .HasColumnName("fManagerPassword");
            entity.Property(e => e.FManagerPasswordSalt)
                .IsUnicode(false)
                .HasColumnName("fManagerPasswordSalt");
            entity.Property(e => e.FManagerPermissionState).HasColumnName("fManagerPermissionState");
            entity.Property(e => e.FManagerPhone)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("fManagerPhone");
            entity.Property(e => e.FManagerProductState).HasColumnName("fManagerProductState");
            entity.Property(e => e.FManagerServiceState).HasColumnName("fManagerServiceState");
            entity.Property(e => e.FManagerState).HasColumnName("fManagerState");
            entity.Property(e => e.FManagerUserState).HasColumnName("fManagerUserState");
        });

        modelBuilder.Entity<TMatch>(entity =>
        {
            entity.HasKey(e => e.FMatchId).HasName("PK__tMatch__6E31C126FB29B207");

            entity.ToTable("tMatch");

            entity.Property(e => e.FMatchId).HasColumnName("fMatchId");
            entity.Property(e => e.FFrom).HasColumnName("fFrom");
            entity.Property(e => e.FMatchDate)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("fMatchDate");
            entity.Property(e => e.FMessage)
                .HasMaxLength(50)
                .HasColumnName("fMessage");
            entity.Property(e => e.FMessageStatus)
                .HasMaxLength(20)
                .HasColumnName("fMessageStatus");
            entity.Property(e => e.FStatusId).HasColumnName("fStatusId");
            entity.Property(e => e.FTo).HasColumnName("fTo");
            entity.Property(e => e.FUpdateDate)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("fUpdateDate");

            entity.HasOne(d => d.FFromNavigation).WithMany(p => p.TMatchFFromNavigations)
                .HasForeignKey(d => d.FFrom)
                .HasConstraintName("FK_tMatch_tUserPrefer");

            entity.HasOne(d => d.FStatus).WithMany(p => p.TMatches)
                .HasForeignKey(d => d.FStatusId)
                .HasConstraintName("FK_tMatch_tStatus");

            entity.HasOne(d => d.FToNavigation).WithMany(p => p.TMatchFToNavigations)
                .HasForeignKey(d => d.FTo)
                .HasConstraintName("FK_tMatch_tUserPrefer1");
        });

        modelBuilder.Entity<TOrder>(entity =>
        {
            entity.HasKey(e => e.FOrderId).HasName("PK__tOrder__5DA911FA56AED620");

            entity.ToTable("tOrder");

            entity.Property(e => e.FOrderId).HasColumnName("fOrderId");
            entity.Property(e => e.FOrderDate)
                .HasMaxLength(20)
                .HasColumnName("fOrderDate");
            entity.Property(e => e.FOrderRemarks)
                .HasMaxLength(50)
                .HasColumnName("fOrderRemarks");
            entity.Property(e => e.FOrderStatus)
                .HasMaxLength(20)
                .HasColumnName("fOrderStatus");
            entity.Property(e => e.FPaymentInfo)
                .HasMaxLength(20)
                .HasColumnName("fPaymentInfo");
            entity.Property(e => e.FPaymentStatus).HasColumnName("fPaymentStatus");
            entity.Property(e => e.FRecepientAddress)
                .HasMaxLength(50)
                .HasColumnName("fRecepientAddress");
            entity.Property(e => e.FRecepientEmail)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("fRecepientEmail");
            entity.Property(e => e.FRecepientName)
                .HasMaxLength(20)
                .HasColumnName("fRecepientName");
            entity.Property(e => e.FRecepientPhone)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("fRecepientPhone");
            entity.Property(e => e.FReturnInfo)
                .HasMaxLength(20)
                .HasColumnName("fReturnInfo");
            entity.Property(e => e.FServiceStatus).HasColumnName("fServiceStatus");
            entity.Property(e => e.FShippingInfo)
                .HasMaxLength(20)
                .HasColumnName("fShippingInfo");
            entity.Property(e => e.FShippingStatus).HasColumnName("fShippingStatus");
            entity.Property(e => e.FTotalPrice)
                .HasColumnType("money")
                .HasColumnName("fTotalPrice");
            entity.Property(e => e.FUserId).HasColumnName("fUserId");
        });

        modelBuilder.Entity<TOrderDetail>(entity =>
        {
            entity.HasKey(e => e.FOrderDetailId).HasName("PK__tOrderDe__59429BAF5B035932");

            entity.ToTable("tOrderDetail");

            entity.Property(e => e.FOrderDetailId).HasColumnName("fOrderDetailId");
            entity.Property(e => e.FOrderId).HasColumnName("fOrderId");
            entity.Property(e => e.FPrice)
                .HasColumnType("money")
                .HasColumnName("fPrice");
            entity.Property(e => e.FProductId).HasColumnName("fProductId");
            entity.Property(e => e.FProductReview)
                .HasMaxLength(255)
                .HasColumnName("fProductReview");
            entity.Property(e => e.FQuantity).HasColumnName("fQuantity");
            entity.Property(e => e.FReviewDate)
                .HasMaxLength(20)
                .HasColumnName("fReviewDate");
            entity.Property(e => e.FReviewScore).HasColumnName("fReviewScore");
        });

        modelBuilder.Entity<TPermission>(entity =>
        {
            entity.HasKey(e => e.FPermissionId).HasName("PK__tPermiss__B5267B178066805B");

            entity.ToTable("tPermission");

            entity.Property(e => e.FPermissionId).HasColumnName("fPermissionId");
            entity.Property(e => e.FPermissionDescription)
                .HasMaxLength(255)
                .HasColumnName("fPermissionDescription");
            entity.Property(e => e.FPermissionName)
                .HasMaxLength(50)
                .HasColumnName("fPermissionName");
        });

        modelBuilder.Entity<TPost>(entity =>
        {
            entity.HasKey(e => e.PoId).HasName("PK_Posts");

            entity.ToTable("tPosts");

            entity.Property(e => e.ClickCount).HasDefaultValue(0);
            entity.Property(e => e.CommentCount).HasDefaultValue(0);
            entity.Property(e => e.PoContent).IsRequired();
            entity.Property(e => e.PoTitle).IsRequired();
            entity.Property(e => e.UserId).HasMaxLength(450);

            entity.HasOne(d => d.For).WithMany(p => p.TPosts)
                .HasForeignKey(d => d.ForId)
                .HasConstraintName("FK_Posts_Forums_ForumForId");
        });

        modelBuilder.Entity<TPostReply>(entity =>
        {
            entity.HasKey(e => e.ReId).HasName("PK_PostReplies");

            entity.ToTable("tPostReplies");

            entity.Property(e => e.ReContent).IsRequired();
            entity.Property(e => e.UserId).HasMaxLength(450);
        });

        modelBuilder.Entity<TProdCategory>(entity =>
        {
            entity.HasKey(e => e.FProdCategoryId).HasName("PK__tProdCat__85859DD46DD2E5B5");

            entity.ToTable("tProdCategory");

            entity.Property(e => e.FProdCategoryId).HasColumnName("fProdCategoryId");
            entity.Property(e => e.FProdCatName)
                .HasMaxLength(50)
                .HasColumnName("fProdCatName");
        });

        modelBuilder.Entity<TProduct>(entity =>
        {
            entity.HasKey(e => e.FProductId).HasName("PK__tProduct__6168D8E0289EA4AB");

            entity.ToTable("tProduct");

            entity.Property(e => e.FProductId).HasColumnName("fProductId");
            entity.Property(e => e.FCompanyId).HasColumnName("fCompanyId");
            entity.Property(e => e.FProdCategory)
                .HasMaxLength(50)
                .HasColumnName("fProdCategory");
            entity.Property(e => e.FProdCreateAt)
                .HasMaxLength(50)
                .HasColumnName("fProdCreateAt");
            entity.Property(e => e.FProdDescription)
                .HasMaxLength(100)
                .HasColumnName("fProdDescription");
            entity.Property(e => e.FProdImage)
                .HasMaxLength(100)
                .HasColumnName("fProdImage");
            entity.Property(e => e.FProdName)
                .HasMaxLength(50)
                .HasColumnName("fProdName");
            entity.Property(e => e.FProdPrice)
                .HasColumnType("money")
                .HasColumnName("fProdPrice");
            entity.Property(e => e.FProdPromoEndDate)
                .HasMaxLength(50)
                .HasColumnName("fProdPromoEndDate");
            entity.Property(e => e.FProdPromoStartDate)
                .HasMaxLength(50)
                .HasColumnName("fProdPromoStartDate");
            entity.Property(e => e.FProdSellingPrice)
                .HasColumnType("money")
                .HasColumnName("fProdSellingPrice");
            entity.Property(e => e.FProdStatus).HasColumnName("fProdStatus");
            entity.Property(e => e.FProdStock).HasColumnName("fProdStock");
            entity.Property(e => e.FProdUpdateAt)
                .HasMaxLength(50)
                .HasColumnName("fProdUpdateAt");
        });

        modelBuilder.Entity<TQuestionAnswer>(entity =>
        {
            entity.HasKey(e => e.FQuestionId).HasName("PK__tQuestio__725221DD4B6E3CB0");

            entity.ToTable("tQuestionAnswer");

            entity.Property(e => e.FQuestionId).HasColumnName("fQuestionId");
            entity.Property(e => e.FAnswer).HasColumnName("fAnswer");
            entity.Property(e => e.FQuestion).HasColumnName("fQuestion");
            entity.Property(e => e.FQuestionTypesId).HasColumnName("fQuestionTypesID");
        });

        modelBuilder.Entity<TQuestionType>(entity =>
        {
            entity.HasKey(e => e.FQuestionTypesId).HasName("PK__tQuestio__00A10BBAA284AEB6");

            entity.ToTable("tQuestionType");

            entity.Property(e => e.FQuestionTypesId).HasColumnName("fQuestionTypesId");
            entity.Property(e => e.FQuestionTypes)
                .HasMaxLength(50)
                .HasColumnName("fQuestionTypes");
        });

        modelBuilder.Entity<TRegDetail>(entity =>
        {
            entity.HasKey(e => e.FRegDetailsId);

            entity.ToTable("tRegDetails");

            entity.Property(e => e.FRegDetailsId).HasColumnName("fRegDetailsId");
            entity.Property(e => e.FRecipientAddress)
                .HasMaxLength(255)
                .HasColumnName("fRecipientAddress");
            entity.Property(e => e.FRegEmail)
                .HasMaxLength(255)
                .HasColumnName("fRegEmail");
            entity.Property(e => e.FRegId).HasColumnName("fRegId");
            entity.Property(e => e.FRegName)
                .HasMaxLength(20)
                .HasColumnName("fRegName");
            entity.Property(e => e.FRegTel)
                .HasMaxLength(40)
                .HasColumnName("fRegTel");

            entity.HasOne(d => d.FReg).WithMany(p => p.TRegDetails)
                .HasForeignKey(d => d.FRegId)
                .HasConstraintName("FK_tRegDetails_tEventReg");
        });

        modelBuilder.Entity<TServiceDetail>(entity =>
        {
            entity.HasKey(e => e.FServiceId).HasName("PK__tService__5345E9AE9FCBCAD0");

            entity.ToTable("tServiceDetail");

            entity.Property(e => e.FServiceId).HasColumnName("fServiceId");
            entity.Property(e => e.FOrderId).HasColumnName("fOrderId");
            entity.Property(e => e.FProductId).HasColumnName("fProductId");
            entity.Property(e => e.FServiceDate)
                .HasMaxLength(50)
                .HasColumnName("fServiceDate");
            entity.Property(e => e.FServiceDescription).HasColumnName("fServiceDescription");
            entity.Property(e => e.FServicePerson)
                .HasMaxLength(50)
                .HasColumnName("fServicePerson");
            entity.Property(e => e.FTargetName)
                .HasMaxLength(50)
                .HasColumnName("fTargetName");
            entity.Property(e => e.FUserId).HasColumnName("fUserId");
        });

        modelBuilder.Entity<TStatus>(entity =>
        {
            entity.HasKey(e => e.FStatusId);

            entity.ToTable("tStatus");

            entity.Property(e => e.FStatusId).HasColumnName("fStatusId");
            entity.Property(e => e.FCategory)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("fCategory");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsFixedLength();
        });

        modelBuilder.Entity<TStockDetail>(entity =>
        {
            entity.HasKey(e => e.FStockDetailId).HasName("PK__tStockDe__D4E1C649B684EFDC");

            entity.ToTable("tStockDetail");

            entity.Property(e => e.FStockDetailId).HasColumnName("fStockDetailId");
            entity.Property(e => e.FChangeAmount).HasColumnName("fChangeAmount");
            entity.Property(e => e.FChangeDateTime)
                .HasMaxLength(50)
                .HasColumnName("fChangeDateTime");
            entity.Property(e => e.FChangeType)
                .HasMaxLength(50)
                .HasColumnName("fChangeType");
            entity.Property(e => e.FProductId).HasColumnName("fProductId");
        });

        modelBuilder.Entity<TTrait>(entity =>
        {
            entity.HasKey(e => e.FTraitsId).HasName("PK__tTraits__9E0BD7E6CC593A68");

            entity.ToTable("tTraits");

            entity.Property(e => e.FTraitsId).HasColumnName("fTraitsId");
            entity.Property(e => e.FTraitsName)
                .HasMaxLength(50)
                .HasColumnName("fTraitsName");
        });

        modelBuilder.Entity<TTraitsPrefer>(entity =>
        {
            entity.HasKey(e => e.FTraitsPreferId).HasName("PK__tTraitsP__5BA4EE40F4F7A5BA");

            entity.ToTable("tTraitsPrefer");

            entity.Property(e => e.FTraitsPreferId).HasColumnName("fTraitsPreferId");
            entity.Property(e => e.FTraitsId).HasColumnName("fTraitsId");
            entity.Property(e => e.FUserPreferId).HasColumnName("fUserPreferId");

            entity.HasOne(d => d.FTraits).WithMany(p => p.TTraitsPrefers)
                .HasForeignKey(d => d.FTraitsId)
                .HasConstraintName("FK_tTraitsPrefer_tTraits");

            entity.HasOne(d => d.FUserPrefer).WithMany(p => p.TTraitsPrefers)
                .HasForeignKey(d => d.FUserPreferId)
                .HasConstraintName("FK_tTraitsPrefer_tUserPrefer");
        });

        modelBuilder.Entity<TUpcomingAct>(entity =>
        {
            entity.HasKey(e => e.FAnnouncementId).HasName("PK__tUpcomin__231916E8CC69DD28");

            entity.ToTable("tUpcomingAct");

            entity.Property(e => e.FAnnouncementId).HasColumnName("fAnnouncementId");
            entity.Property(e => e.FActId).HasColumnName("fActId");
            entity.Property(e => e.FActUrl).HasColumnName("fActUrl");
            entity.Property(e => e.FAnnounceImg)
                .HasColumnType("image")
                .HasColumnName("fAnnounceImg");
            entity.Property(e => e.FApplicationDate)
                .HasMaxLength(20)
                .HasColumnName("fApplicationDate");
            entity.Property(e => e.FReleaseDate)
                .HasMaxLength(20)
                .HasColumnName("fReleaseDate");
            entity.Property(e => e.FRemovalDate)
                .HasMaxLength(20)
                .HasColumnName("fRemovalDate");
        });

        modelBuilder.Entity<TUser>(entity =>
        {
            entity.HasKey(e => e.FUserId).HasName("PK__tUser__A48DC693C29A44E8");

            entity.ToTable("tUser");

            entity.Property(e => e.FUserId).HasColumnName("fUserId");
            entity.Property(e => e.FUserAddress)
                .HasMaxLength(255)
                .HasColumnName("fUserAddress");
            entity.Property(e => e.FUserBirthdate).HasColumnName("fUserBirthdate");
            entity.Property(e => e.FUserEmail)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("fUserEmail");
            entity.Property(e => e.FUserGender).HasColumnName("fUserGender");
            entity.Property(e => e.FUserImage)
                .HasMaxLength(255)
                .HasColumnName("fUserImage");
            entity.Property(e => e.FUserName)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("fUserName");
            entity.Property(e => e.FUserNickName)
                .HasMaxLength(50)
                .HasColumnName("fUserNickName");
            entity.Property(e => e.FUserPassword).HasColumnName("fUserPassword");
            entity.Property(e => e.FUserPasswordSalt).HasColumnName("fUserPasswordSalt");
            entity.Property(e => e.FUserPhone)
                .HasMaxLength(50)
                .HasColumnName("fUserPhone");
            entity.Property(e => e.FUserStateId).HasColumnName("fUserStateId");

            entity.HasOne(d => d.FUserState).WithMany(p => p.TUsers)
                .HasForeignKey(d => d.FUserStateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tUser_tUserState");
        });

        modelBuilder.Entity<TUserHobby>(entity =>
        {
            entity.HasKey(e => e.FUserHobbyId).HasName("PK__tUserHob__9D939A2511D0DED2");

            entity.ToTable("tUserHobby");

            entity.Property(e => e.FUserHobbyId).HasColumnName("fUserHobbyId");
            entity.Property(e => e.FHobbyId).HasColumnName("fHobbyId");
            entity.Property(e => e.FUserPreferId).HasColumnName("fUserPreferId");

            entity.HasOne(d => d.FHobby).WithMany(p => p.TUserHobbies)
                .HasForeignKey(d => d.FHobbyId)
                .HasConstraintName("FK_tUserHobby_tHobby");

            entity.HasOne(d => d.FUserPrefer).WithMany(p => p.TUserHobbies)
                .HasForeignKey(d => d.FUserPreferId)
                .HasConstraintName("FK_tUserHobby_tUserPrefer");
        });

        modelBuilder.Entity<TUserLike>(entity =>
        {
            entity.HasKey(e => e.FUserLikeId);

            entity.ToTable("tUserLike");

            entity.Property(e => e.FUserLikeId).HasColumnName("fUserLikeId");
            entity.Property(e => e.FProductId).HasColumnName("fProductId");
            entity.Property(e => e.FUserId).HasColumnName("fUserId");

            entity.HasOne(d => d.FProduct).WithMany(p => p.TUserLikes)
                .HasForeignKey(d => d.FProductId)
                .HasConstraintName("FK_tUserLike_tProduct");

            entity.HasOne(d => d.FUser).WithMany(p => p.TUserLikes)
                .HasForeignKey(d => d.FUserId)
                .HasConstraintName("FK_tUserLike_tUser");
        });

        modelBuilder.Entity<TUserPrefer>(entity =>
        {
            entity.HasKey(e => e.FUserPreferId).HasName("PK__tUserPre__28F1E3A174315F25");

            entity.ToTable("tUserPrefer");

            entity.Property(e => e.FUserPreferId).HasColumnName("fUserPreferId");
            entity.Property(e => e.FAge).HasColumnName("fAge");
            entity.Property(e => e.FCity)
                .HasMaxLength(50)
                .HasColumnName("fCity");
            entity.Property(e => e.FGender).HasColumnName("fGender");
            entity.Property(e => e.FHeight)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("fHeight");
            entity.Property(e => e.FInfo).HasColumnName("fInfo");
            entity.Property(e => e.FMaxAge).HasColumnName("fMaxAge");
            entity.Property(e => e.FMaxHeight)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("fMaxHeight");
            entity.Property(e => e.FMinAge).HasColumnName("fMinAge");
            entity.Property(e => e.FMinHeight)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("fMinHeight");
            entity.Property(e => e.FPhotoPath).HasColumnName("fPhotoPath");
            entity.Property(e => e.FUserId).HasColumnName("fUserId");

            entity.HasOne(d => d.FUser).WithMany(p => p.TUserPrefers)
                .HasForeignKey(d => d.FUserId)
                .HasConstraintName("FK_tUserPrefer_tUser");
        });

        modelBuilder.Entity<TUserState>(entity =>
        {
            entity.HasKey(e => e.FUserStateId);

            entity.ToTable("tUserState");

            entity.Property(e => e.FUserStateId).HasColumnName("fUserStateId");
            entity.Property(e => e.FUserState)
                .HasMaxLength(10)
                .HasColumnName("fUserState");
        });

        modelBuilder.Entity<TUserTrait>(entity =>
        {
            entity.HasKey(e => e.FUserTraitsId).HasName("PK__tUserTra__682F6C9C9DB92BDD");

            entity.ToTable("tUserTraits");

            entity.Property(e => e.FUserTraitsId).HasColumnName("fUserTraitsId");
            entity.Property(e => e.FTraitsId).HasColumnName("fTraitsId");
            entity.Property(e => e.FUserPreferId).HasColumnName("fUserPreferId");

            entity.HasOne(d => d.FTraits).WithMany(p => p.TUserTraits)
                .HasForeignKey(d => d.FTraitsId)
                .HasConstraintName("FK_tUserTraits_tTraits");

            entity.HasOne(d => d.FUserPrefer).WithMany(p => p.TUserTraits)
                .HasForeignKey(d => d.FUserPreferId)
                .HasConstraintName("FK_tUserTraits_tUserPrefer");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}