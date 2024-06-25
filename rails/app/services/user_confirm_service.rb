# app/services/user_confirm_service.rb
class UserConfirmService
  def initialize(confirmation_token)
    @token = confirmation_token
    @user = User.find_by(confirmation_token: confirmation_token)
  end

  def call
    if @user
      if @user.confirmed_at != nil
        logger.info "User with email #{@user.email} already confirmed"
        return
      end
      was_confirmed = @user.confirm
      was_confirmed ? logger.info("User with email #{@user.email} confirmed")
        : logger.error("User with email #{@user.email} was not confirmed")
    else
      logger.error "User with confirmation token #{@token} could not be found"
      throw "User not found"
    end
  end

  private

  def logger
    return @logger ||= Logger.new(Rails.root.join('log', 'user_confirm.log'))
  end
end