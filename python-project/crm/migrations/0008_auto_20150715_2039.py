# -*- coding: utf-8 -*-
from __future__ import unicode_literals

from django.db import models, migrations
from decimal import Decimal
import phonenumber_field.modelfields


class Migration(migrations.Migration):

    dependencies = [
        ('crm', '0007_auto_20150612_1602'),
    ]

    operations = [
        migrations.CreateModel(
            name='People',
            fields=[
                ('id', models.AutoField(verbose_name='ID', serialize=False, auto_created=True, primary_key=True)),
                ('contact_name', models.CharField(max_length=300)),
                ('email', models.EmailField(max_length=75, null=True, blank=True)),
                ('title', models.CharField(max_length=300, null=True, blank=True)),
                ('phone_number', phonenumber_field.modelfields.PhoneNumberField(max_length=128, blank=True)),
                ('time_stamp', models.DateTimeField(auto_now_add=True)),
                ('company', models.ForeignKey(to='crm.Customer')),
            ],
            options={
            },
            bases=(models.Model,),
        ),
        migrations.RemoveField(
            model_name='customer',
            name='contact_name',
        ),
        migrations.RemoveField(
            model_name='customer',
            name='email',
        ),
        migrations.RemoveField(
            model_name='customer',
            name='title',
        ),
        migrations.AddField(
            model_name='customer',
            name='status',
            field=models.CharField(default=b'prospect', max_length=20, choices=[(b'prospect', b'prospect'), (b'contacted', b'contacted'), (b'qualified', b'qualified'), (b'Customer', b'Customer')]),
            preserve_default=True,
        ),
        migrations.AddField(
            model_name='customer',
            name='value',
            field=models.DecimalField(default=Decimal('0.00'), max_digits=10, decimal_places=2),
            preserve_default=True,
        ),
    ]
