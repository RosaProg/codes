#newmarkets/admin.py
from __future__ import unicode_literals
from django.contrib import admin
from newmarkets.models import Country, HSProduct, IntracenImportData

# Register your models here.
admin.site.register(Country)
admin.site.register(HSProduct)
admin.site.register(IntracenImportData)