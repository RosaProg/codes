#newmarkets/models.py
from __future__ import unicode_literals
from django.db import models

# Create your models here.

class HSProduct(models.Model):
    """
    Harmonized Tarriff code. HTS or HS. Sourced from Intracen

    """
    hs_number = models.CharField(primary_key=True, blank=True, max_length=10)
    description = models.CharField(max_length=250)

    def __unicode__(self):
        return self.hs_number

class Country(models.Model):
    """
    Imagine if there were no countries...

    """
    iso2code = models.CharField(max_length=2, primary_key= True)
    iso3code = models.CharField(max_length=3)
    name = models.CharField(max_length=200)
    capital = models.CharField(max_length=200)
    longitude = models.DecimalField(max_digits=9, decimal_places=6, null=True)
    latitude = models.DecimalField(max_digits=9, decimal_places=6, null=True)

    created_at = models.DateTimeField(auto_now_add=True)
    updated_at = models.DateTimeField(auto_now=True)
    
    country = models.ManyToManyField(HSProduct, through="IntracenImportData")

    def __unicode__(self):
        return self.name

class IntracenImportData(models.Model):
    """
    Import data from intracen 

    """

    hs_number = models.ForeignKey(HSProduct)
    country = models.ForeignKey(Country)
    imported_value = models.DecimalField(max_digits=12, decimal_places=2, default=0.00, null = True)  
    imported_time = models.PositiveSmallIntegerField()