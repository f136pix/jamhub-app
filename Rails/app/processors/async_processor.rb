class AsyncProcessor

  # uses same async processor lazy initialized in action_subscriber.rb
  SERVICE_MAPPING = {
    'email.confirmation' => ->(message) { async_processor.process_email_confirm(message) },
    # 'routing_key_2' => ->(message) { ServiceClass2.new(message).call },
    # Add more routing keys and corresponding service classes/methods as needed
  }

  def process(message, routing_key)
    begin
      logger.info "Processing message: #{message}, routing to #{routing_key}" 
      service = SERVICE_MAPPING[routing_key]
      service.call(message) if service
    rescue => e
      logger.info "An error occurred: #{e.message}"
    else
      logger.info "No errors occurred."
    ensure
      # finally
      logger.info "Processing finished"
      logger.info "#{'-' * 50}"

    end
  end

  def process_email_confirm(json_information_message)
    json_message = JSON.parse(json_information_message)
    confirmation_token = json_message['ConfirmationToken']
    puts "Confirmation Token: #{confirmation_token}"

    UserConfirmService.new(confirmation_token).call
    
    # no need to delete, UserConfirmService garbage collector does it
  end

  private

  def logger
    @logger ||= Logger.new(Rails.root.join('log', 'consumer.log'))
  end

end