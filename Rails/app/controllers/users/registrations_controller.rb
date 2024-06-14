require 'async_publisher'

class Users::RegistrationsController < Devise::RegistrationsController
  respond_to :json

  # POST /resource
  def create
    super
  end

  private def respond_with(resource, _opts = {})
    resource.persisted? ? register_success : register_failed
  end

  def register_success
    message = {
      email: resource.email,
      confirmation_token: resource.confirmation_token
    }
    # sends user email to dotnet server so it does the job there
    # direct exchange with routing key user.registered
    AsyncPublisher.publish('ampq.direct', 'user.registered', message, 'rails.dotnet')
    render json: { message: 'Signed up.' }
  end

  def register_failed
    render json: { message: "Signed up failure." }
  end
end