#projects/models.py

from django.conf import settings
from django.contrib.auth.models import User
from django.db import models
from django.db.models import signals
from django.utils import timezone
from django.utils.translation import ugettext_lazy as _

from django_countries.fields import CountryField

from autoslug import AutoSlugField
from filer.fields.file import FilerFileField
from phonenumber_field.modelfields import PhoneNumberField


class Customer(models.Model):
    """
    UPS Customer
    
    """
    user = models.ForeignKey(User)

    company_name =  models.CharField(max_length=300)
    slug = AutoSlugField(populate_from='company_name', blank=True, null=True)
    website = models.URLField(max_length=200, null=True, blank=True)
    address1 = models.CharField(max_length=300, null=True, blank=True)
    address2 = models.CharField(max_length=300, null=True, blank=True)
    city = models.CharField(max_length=100)
    state_province = models.CharField(max_length=300, null=True, blank=True)
    postal_code = models.CharField(max_length=10, null=True, blank=True)
    country = CountryField()
    
    description = models.TextField(null=False, blank=False)
    
    created_at = models.DateTimeField(null=False, blank=False,
                                        verbose_name=_("created date"),
                                        default=timezone.now)
    modified_at = models.DateTimeField(null=True, blank=True,
                                         verbose_name=_("modified date"))
    
class Contact(models.Model):
    """
    This class represent people who are associated with the company. 
    Contact names
    """
    company = models.ForeignKey(Customer)
    contact_name = models.CharField(max_length=300)
    email = models.EmailField(null=True, blank=True)
    title = models.CharField(max_length=300, null=True, blank=True)
    phone_number = PhoneNumberField(blank=True)
    skype = models.CharField(max_length=50, null=True, blank=True)
    whatsapp = models.CharField(max_length=50, null=True, blank=True)
    time_stamp = models.DateTimeField(auto_now_add=True)

class Attachment(models.Model):
    """
    Saving files related to the owned_projects.

    """
    owner = models.ForeignKey(settings.AUTH_USER_MODEL, null=False, blank=False)
    file_name = models.CharField(blank=True, null=True, max_length=500)
    attached_file = models.FileField(upload_to='attachments/%Y/%m/%d')
    description = models.TextField(null=True, blank=True)
    created_at = models.DateTimeField(null=False, blank=False,
                                        verbose_name=_("created date"),
                                        default=timezone.now)
    

class Post(models.Model):
    author = models.ForeignKey(User)
    text = models.TextField()

    # Time is a rhinocerous
    updated = models.DateTimeField(auto_now=True)
    created = models.DateTimeField(auto_now_add=True)

    class Meta:
        ordering = ['created']

    def __unicode__(self):
        return self.text+' - '+self.author.username
    


