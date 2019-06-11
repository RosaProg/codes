# -*- coding: utf-8 -*-
from __future__ import unicode_literals

from django.db import models, migrations
import django_countries.fields


class Migration(migrations.Migration):

    dependencies = [
    ]

    operations = [
        migrations.CreateModel(
            name='Customer',
            fields=[
                ('id', models.AutoField(verbose_name='ID', serialize=False, auto_created=True, primary_key=True)),
                ('company_name', models.CharField(max_length=300)),
                ('website', models.URLField(null=True, blank=True)),
                ('address1', models.CharField(max_length=300, null=True, blank=True)),
                ('address2', models.CharField(max_length=300, null=True, blank=True)),
                ('city', models.CharField(max_length=100)),
                ('state_province', models.CharField(max_length=300, null=True, blank=True)),
                ('country', django_countries.fields.CountryField(max_length=2)),
                ('contact_name', models.CharField(max_length=300)),
                ('title', models.CharField(max_length=300, null=True, blank=True)),
                ('time_stamp', models.DateTimeField(auto_now_add=True)),
            ],
            options={
            },
            bases=(models.Model,),
        ),
        migrations.CreateModel(
            name='Interaction',
            fields=[
                ('id', models.AutoField(verbose_name='ID', serialize=False, auto_created=True, primary_key=True)),
                ('when', models.DateTimeField(auto_now_add=True)),
                ('kind', models.IntegerField(choices=[(1, 'Phone Call'), (2, 'Email')])),
                ('note', models.TextField(null=True, blank=True)),
                ('customer', models.ForeignKey(to='crm.Customer')),
            ],
            options={
            },
            bases=(models.Model,),
        ),
    ]
