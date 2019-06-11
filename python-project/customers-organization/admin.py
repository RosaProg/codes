from django.contrib import admin

from .models import CustomerOrganization, CustomerOrganizationUser, CustomerOrganizationOwner


admin.site.register(CustomerOrganization)
admin.site.register(CustomerOrganizationUser)
admin.site.register(CustomerOrganizationOwner)
