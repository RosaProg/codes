# views from the main site
from django.contrib.auth.decorators import login_required
from django.shortcuts import render, redirect
from .forms import ProfileForm, CompanyForm
from company.models import Company
from myusers.models import ProfileModel
from django.http import JsonResponse, HttpResponse
from django.contrib.auth.models import User
from organizations.utils import create_organization
from pprint import pprint

def promo(request):
    return render(request, 'promo.html')

def topcountries(request):
    return render(request, 'topcountries.html')

def topcountries2(request):
    return render(request, 'topcountries2.html')

def settings(request):
	# needed to get the user profile saved, by user logged in
	user_name = None
	if request.user.is_authenticated():
		user_name = request.user.username
		# We get the Company settings data
		organization_data = Company.objects.filter(username=request.user.username,is_active=True)
		# We get the Company users data
		organization_users = ProfileModel.objects.filter(user=organization_data.user)
		
	context = {
		'current_user':request.user.id,
		'username':request.user.username,
		'first_name':request.user.first_name,
		'last_name':request.user.last_name,
		'email':request.user.email,
		'admin':request.user.is_superuser,
		'last_action':request.user.last_login
	}
	
	print request.user.email
	"""
	else:
		user_name = None
	try:	
		current_user = User.objects.get(username=user_name)
		try:
			test = Company.objects.get(user=current_user)
		except Company.DoesNotExist:
			test = None
			print 'In Company the current user does not exist'
			
	except User.DoesNotExist:
		current_user = None
		print 'User not found'
	
	
	print test
	"""
	return render(request, 'account/settings_overview.html', context)

def settings_profile(request):
	form = ProfileForm()
	test = request.POST.get("first_name","")
	print request.POST.get('first_name',"")
	print request.POST.get('last_name',"")
	print request.POST.get('phone',"")
	print request.POST.get('timezone',"")
	print request.POST.get('language',"")
	print request.FILES.get('docfile', "")
	form = ProfileForm(request.POST, request.FILES)
	if form.is_valid():
		my_model = form.save() #commit=False
	else:
		form = ProfileForm()
	try:
		response_data = JsonResponse({'result': 'Saved successfully.', 'message': test})
	except:
		response_data = JsonResponse({'result': 'Error', 'message': 'The process did not run correctly.'})

	return HttpResponse(response_data, content_type="application/json")
	
def settings_company(request):
	form = CompanyForm()
	test = request.POST.get("company_name","")
	print request.POST.get("company_name","")
	response_data = JsonResponse({'result': 'Saved successfully.', 'message': test})
	form = CompanyForm(request.POST)
	if form.is_valid():
		my_model = form.save() # commit=False
	else:
		form = CompanyForm()
	try:
		response_data = JsonResponse({'result': 'Saved successfully.', 'message': test})
	except:
		response_data = JsonResponse({'result': 'Error', 'message': 'The process did not run correctly.'})

	return HttpResponse(response_data, content_type="application/json")
	
def users_permission(request):
	# needed to get all company users filtered by company. company will be got from 'company_name' field of settings_overview.html
	response_data = JsonResponse({'result': 'Saved successfully.', 'message': test})
	return HttpResponse(response_data, content_type="application/json")

def organization_admin(request):
	# Update to ProfileModel with is_admin = True
	test = request.POST.get("company","aaa")
	test1 = request.POST.get("user","")
	print test
	print test1
	response_data = JsonResponse({'result': 'Initiating.', 'message': test})
	"""
	>>> from blog.models import Entry
	>>> entry = Entry.objects.get(pk=1)
	>>> cheese_blog = Blog.objects.get(name="Cheddar Talk")
	>>> entry.blog = cheese_blog
	>>> entry.save()
	"""
	
	# Search for the user by the email, and if found, get the id to save into is_admin in ProfileModel
	#try:
	print "Comienza proceso de guardar organization admin"
	print "Email: " + test1
	pprint(User.objects.filter(username=test1,is_active=True));
	admin_user = User.objects.filter(username=test1,is_active=True)
	print "Actual user id"
	print request.user.id
	print "admin_user value"
	for admin in admin_user:
		print(admin.username)
		print(admin.id)
		print(admin.email)
		pprint(ProfileModel.objects.get(user=admin.id))
		user_account = ProfileModel.objects.get(user=admin.id)
		print "user account from profile model"
		for account in user_account:
			print account.id
			account.is_admin = True
			account.save()
	
	response_data = JsonResponse({'result': 'User assigned as admin succesfully.', 'message': test})
	
	
	#except:
	#	response_data = JsonResponse({'result': 'Error', 'message': 'The user needs to register before being assigned as admin.'})

	#form = ProfileForm()
	
	return HttpResponse(response_data, content_type="application/json")
	
def blog(request):
    return redirect('http://blog.exportabroad.com/')

@login_required
def dashboard(request):
    return render(request, 'dashboard.html')
