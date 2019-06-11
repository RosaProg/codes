# -*- coding: utf-8 -*-
from __future__ import unicode_literals

from django.db import models, migrations
import autoslug.fields
from django.conf import settings


class Migration(migrations.Migration):

    dependencies = [
        ('crm', '0005_merge'),
    ]

    operations = [
        migrations.AddField(
            model_name='customer',
            name='slug',
            field=autoslug.fields.AutoSlugField(populate_from=b'company_name', null=True, editable=False, blank=True),
            preserve_default=True,
        ),
        migrations.AlterField(
            model_name='customer',
            name='user',
            field=models.ForeignKey(to=settings.AUTH_USER_MODEL),
        ),
    ]
