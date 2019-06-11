# -*- coding: utf-8 -*-
from __future__ import unicode_literals

from django.db import models, migrations
import autoslug.fields
from decimal import Decimal
import django_countries.fields
from django.conf import settings


class Migration(migrations.Migration):

    dependencies = [
        migrations.swappable_dependency(settings.AUTH_USER_MODEL),
        ('crm', '0009_auto_20150721_1407'),
    ]

    operations = [
        migrations.CreateModel(
            name='Lead',
            fields=[
                ('id', models.AutoField(verbose_name='ID', serialize=False, auto_created=True, primary_key=True)),
                ('company_name', models.CharField(max_length=300)),
                ('slug', autoslug.fields.AutoSlugField(populate_from=b'company_name', null=True, editable=False, blank=True)),
                ('website', models.URLField(null=True, blank=True)),
                ('address1', models.CharField(max_length=300, null=True, blank=True)),
                ('address2', models.CharField(max_length=300, null=True, blank=True)),
                ('city', models.CharField(max_length=100)),
                ('state_province', models.CharField(max_length=300, null=True, blank=True)),
                ('postal_code', models.CharField(max_length=10, null=True, blank=True)),
                ('country', django_countries.fields.CountryField(max_length=2)),
                ('status', models.CharField(default=b'prospect', max_length=20, choices=[(b'prospect', b'prospect'), (b'contacted', b'contacted'), (b'qualified', b'qualified'), (b'Customer', b'Customer')])),
                ('value', models.DecimalField(default=Decimal('0.00'), max_digits=10, decimal_places=2)),
                ('time_stamp', models.DateTimeField(auto_now_add=True)),
                ('user', models.ForeignKey(to=settings.AUTH_USER_MODEL)),
            ],
            options={
            },
            bases=(models.Model,),
        ),
        migrations.RemoveField(
            model_name='customer',
            name='user',
        ),
        migrations.AlterField(
            model_name='interaction',
            name='customer',
            field=models.ForeignKey(to='crm.Lead'),
            preserve_default=True,
        ),
        migrations.AlterField(
            model_name='people',
            name='company',
            field=models.ForeignKey(to='crm.Lead'),
            preserve_default=True,
        ),
        migrations.DeleteModel(
            name='Customer',
        ),
    ]
