﻿using System;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Identity;

namespace Dev.Acadmy.Lectures
{
    public class LectureTry :AuditedAggregateRoot<Guid>
    {
        public Guid LectureId { get; set; }
        public Guid UserId { get; set; }

        [ForeignKey(nameof(LectureId))]
        public Lecture Lecture { get; set; }
        [ForeignKey(nameof(UserId))]
        public IdentityUser User { get; set; }
        public int MyTryCount { get; set; }
        public bool IsSucces { get; set; }

    }
}
