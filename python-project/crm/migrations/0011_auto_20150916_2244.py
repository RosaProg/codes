# -*- coding: utf-8 -*-
from __future__ import unicode_literals

from django.db import models, migrations


class Migration(migrations.Migration):

    dependencies = [
        ('crm', '0010_auto_20150916_1618'),
    ]

    operations = [
        migrations.RenameField(
            model_name='interaction',
            old_name='customer',
            new_name='lead',
        ),
    ]
