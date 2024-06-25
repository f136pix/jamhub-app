class UsersController < ApplicationController
  
  # GET /users
  def index
    render json: User.all
  end
end