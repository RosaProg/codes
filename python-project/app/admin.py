from django.contrib import admin
from newmarkets.models import Countries, Products, Imports
# Register your models here.
admin.site.register(Countries)
admin.site.register(Products)
admin.site.register(Imports)