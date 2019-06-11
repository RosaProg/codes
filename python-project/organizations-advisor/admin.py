from django.contrib import admin

from .models import OrganizationAdvisor, OrganizationAdvisorUser, OrganizationAdvisorOwner


admin.site.register(OrganizationAdvisor)
admin.site.register(OrganizationAdvisorUser)
admin.site.register(OrganizationAdvisorOwner)
