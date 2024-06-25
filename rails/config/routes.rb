Rails.application.routes.draw do

  resources :users
  get '/current_user', to: 'current_user#index'
  devise_for :users,
             path: 'auth',
             path_names: { # overriding default devise paths 
                           sign_in: 'login',
                           sign_out: 'logout',
                           registration: 'register'
             },
             controllers: {
               sessions: 'users/sessions',
               registrations: 'users/registrations'
             }

  # For details on the DSL available within this file, see https://guides.rubyonrails.org/routing.html
end
