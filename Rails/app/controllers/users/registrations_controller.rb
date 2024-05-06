class Users::RegistrationsController < Devise::RegistrationsController
  respond_to :json

  # POST /resource
  def create
    puts "-----> HERE <-----"
    super
  end
  
  private def respond_with(resource, _opts = {})
    resource.persisted? ? register_success : register_failed
  end

  def register_success
    render json: { message: 'Signed up.' }
  end

  def register_failed
    puts "-----> HERE FAILED <-----"
    render json: { message: "Signed up failure." }
  end
end