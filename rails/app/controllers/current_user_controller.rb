class CurrentUserController < ApplicationController
  respond_to :json
  before_action :authenticate_user!

  def index
    if current_user
      render json: current_user, status: :ok
    else
      render json: { error: 'User not logged in' }, status: :unauthorized
    end
  end

  def check
    if current_user
      head :ok
    else
      head :unauthorized
    end
  end

end