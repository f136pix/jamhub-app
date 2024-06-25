class Users::SessionsController < Devise::SessionsController
  # before_action :configure_sign_in_params, only: [:create]
  respond_to :json
  # GET /login
  def new
    super
  end

  # POST /login
  def create
    super
  end

  # DELETE /logout
  def destroy
    if !current_user
      log_out_failure
    else
      super
    end
  end

  # protected

  # If you have extra params to permit, append them to the sanitizer.
  # def configure_sign_in_params
  #   devise_parameter_sanitizer.permit(:sign_in, keys: [:attribute])
  # end

  private

  def respond_with(_resource, _opts = {})
    login_success && return if resource.persisted?

    login_failure
  end

  def login_success
    render json: {
      message: 'You are logged in.',
      user: current_user
    }, status: :ok
  end

  def login_failure
    render json: { message: 'Invalid login credentials.' }, status: :unauthorized
  end

  def respond_to_on_destroy
    log_out_success
  end

  def log_out_success
    auth_header = request.headers['Authorization']
    token = auth_header.split(' ')[1]
    decoded_token = JWT.decode(token, Rails.application.secrets.secret_key_base, false, { algorithm: 'HS256' })
    jti = decoded_token[0]['jti']
    exp = decoded_token[0]['exp']


  message = { jti: jti, exp: exp }
    # post the revoked jit to the queue so the cloned blacklist table can be updated
    AsyncPublisher.publish('ampq.direct', 'token.blacklisted', message, 'rails.dotnet')
    render json: { message: 'User logged out.' }, status: :ok
  end

  def log_out_failure
    render json: { message: 'Cant logout, your are not logged in.' }, status: :unauthorized
  end
end